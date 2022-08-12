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

        private TextBlock framerateText;
        private UniformStackContainer sc;
        private Box box;

        public DebugOverlay()
        {
            
        }

        public override void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;
            Game.Input.KeyUp += Input_KeyUp;
            sc = new UniformStackContainer()
            {
                RelativePosition = new(0, 0, 100, 20),
                Orientation = Orientation.Vertical,
                Layout = Layout.AnchorTopRight,
                Margin = 2
            };
            box = new Box()
            {
                RelativePosition = new(0, 0, 100, 20),
                Color = Color.White * 0.3f,
                Layout = Layout.AnchorBottom
            };
            framerateText = new(new(0, 0, 10, 10), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 15), "", Color.Black)
            {
                Layout = Layout.Fill,
                TextWrapping = TextWrapMode.WordWrap,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Center
            };

            AddChild(sc);
            Game.AddObject(box);
            sc.AddChild(box);
            Game.AddObject(framerateText);
            box.AddChild(framerateText);
        }

        private bool showExtended = false;
        private void Input_KeyUp(object sender, Input.KeyEventArgs e)
        {
            if (e.Key == Keys.LeftControl)
            {
                sc.RelativePosition = new(0, 0, 100, 20);
                box.Color = Color.White * 0.3f;

                showExtended = false;
            }
        }

        private void Input_KeyDown(object sender, Input.KeyEventArgs e)
        {
            if (e.Key == Keys.LeftControl)
            {
                sc.RelativePosition = new(0, 0, 400, 20);
                box.Color = Color.White;

                showExtended = true;      
            }
        }

        public override void Deinitialize()
        {
            Game.Input.KeyDown -= Input_KeyDown;
            Game.Input.KeyUp -= Input_KeyUp;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            drawsPerSecond = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            updatesPerSecond = 1 / gameTime.ElapsedGameTime.TotalSeconds;
            if (showExtended) framerateText.Text = $"{Math.Round(drawsPerSecond, 0)} frames/s, {Math.Round(updatesPerSecond, 0)} updates/s, {Game.AllObjects.Count} objects, {ScreenManager.Screens.Count} screens";
            else framerateText.Text = $"{Math.Round(drawsPerSecond, 0)} fps";

            if (WaterGame.UseExperimentalDrawingMode) framerateText.Text += " EXD";
        }
    }
}
