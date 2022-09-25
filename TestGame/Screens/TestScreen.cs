using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Audio;
using Water.Graphics;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace TestGame.Screens
{
    public class TestScreen : Screen
    {
        private bool x = false;
        private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Space)
            {
                if (x)
                {
                    stackPanel.AddChild(Game.AddObject(new Box()
                    {
                        Color = Color.Black,
                        Margins = new(10, 0)
                    }));
                    x = false;
                }
                else
                {
                    stackPanel.AddChild(Game.AddObject(new Box()
                    {
                        Color = Color.DarkBlue,
                        Margins = new(10, 0)
                    }));
                    x = true;
                }
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.E)
            {
                Game.Audio.SwitchToTrack("Assets/03. Dream Walker.mp3", false);
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.R)
            {
                Game.Audio.PlayEffect("Assets/you_あ、見つかりましたぁ.wav", true, 1, -1);
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.T)
            {
                Game.Audio.PlayEffect("Assets/you_あ、見つかりましたぁ.wav", true, 1, 0);
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.Y)
            {
                Game.Audio.PlayEffect("Assets/you_あ、見つかりましたぁ.wav", true, 1, 1);
            }
        }

        private UniformStackContainer stackPanel;

        public override void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;
            Game.Input.KeyUp += Input_KeyUp;
            stackPanel = new UniformStackContainer()
            {
                RelativePosition = new(0, 0, 100, 100),
                Layout = Layout.Fill,
                Orientation = Orientation.Horizontal,
            };
            AddChild(stackPanel);
        }

        public override void Deinitialize()
        {
            Game.Input.KeyDown -= Input_KeyDown;
            Game.Input.KeyUp -= Input_KeyUp;
        }

        private void Input_KeyUp(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Space)
            {

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }


        public override void Update(GameTime gameTime)
        {

        }
    }
}
