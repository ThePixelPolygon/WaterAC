using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace Water.Screens
{
    public class DebugOverlay : Screen
    {
        private GameObjectManager game;
        private GameWindow window;

        private double updatesPerSecond;
        private double drawsPerSecond;

        private TextBlock updateText;

        public DebugOverlay(GameObjectManager game, GameObjectScreen screen, GameWindow window)
        {
            this.game = game;
            this.window = window;
            var box = new Box(new(0, 0, 500, 200), new(255, 255, 255, 100));
            box.Layout = Layout.AnchorTop;
            updateText = new(new(0, 0, 10, 10), game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 15), "updates per second", Color.Gray)
            {
                Layout = Layout.Fill,
                TextWrapping = TextWrapMode.WordWrap,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Top
            };
            screen.AddChild(box);
            game.AddObject(updateText);
            box.AddChild(updateText);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            drawsPerSecond = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            var intersectedObjects = new List<string>();
            foreach (var obj in game.AllObjects)
            {
                if (new Rectangle(Mouse.GetState(window).Position, new(10, 10)).Intersects(obj.ActualPosition))
                {
                    intersectedObjects.Add($"{obj.ToString().ToUpper()} Actual: {obj.ActualPosition} Relative: {obj.RelativePosition} Parent: {obj.Parent} Children: {string.Join(',', obj.Children)}");
                }
            }

            updatesPerSecond = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            updateText.Text = $"{Math.Round(updatesPerSecond, 2)} updates per second|n{Math.Round(drawsPerSecond, 2)} draws per second|n{game.AllObjects.Count} objects|n{string.Join("|n", intersectedObjects)}";
        }
    }
}
