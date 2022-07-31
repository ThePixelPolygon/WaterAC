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
        private double updatesPerSecond;
        private double drawsPerSecond;

        private TextBlock updateText;
        private string projectName;

        public DebugOverlay()
        {
            
        }

        public override void Initialize()
        {
            var box = new Box(new(0, 0, 500, 200), new(255, 255, 255, 100));
            box.Layout = Layout.AnchorTop;
            updateText = new(new(0, 0, 10, 10), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 15), "updates per second", Color.Gray)
            {
                Layout = Layout.Fill,
                TextWrapping = TextWrapMode.WordWrap,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Top
            };
            ScreenManager.AddChild(box);
            Game.AddObject(updateText);
            box.AddChild(updateText);

            projectName = Window.Title; // lol this is so hacky
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            drawsPerSecond = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            var intersectedObjects = new List<string>();
            foreach (var obj in Game.AllObjects)
            {
                if (new Rectangle(Mouse.GetState(Window).Position, new(10, 10)).Intersects(obj.ActualPosition))
                {
                    intersectedObjects.Add($"{obj.ToString().ToUpper()} Actual: {obj.ActualPosition} Relative: {obj.RelativePosition} Parent: {obj.Parent} Children: {string.Join(',', obj.Children)}");
                }
            }

            updatesPerSecond = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            updateText.Text = $"{Math.Round(updatesPerSecond, 2)} updates per second|n{Math.Round(drawsPerSecond, 2)} draws per second|n{Game.AllObjects.Count} objects|n{string.Join("|n", intersectedObjects)}";
            Window.Title = $"{projectName} - {updateText.Text}";
        }
    }
}
