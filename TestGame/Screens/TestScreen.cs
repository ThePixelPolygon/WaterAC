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
        private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.Space)
            {
                stackPanel.AddChild(Game.AddObject(new Aquarium()
                {
                    Layout = Layout.Fill,
                }));
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.E)
            {
                //var x = new AudioTrack(Game.Audio, @"Assets/03. Dream Walker.mp3");
                //x.Volume = 1f;
                //x.Play();
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.Play(Song.FromUri("03. Dream Walker", new("Assets/03. Dream Walker.ogg", UriKind.Relative)));
            }
            else if (e.Key == Microsoft.Xna.Framework.Input.Keys.R)
            {
                SoundEffect.MasterVolume = 0.5f;
                SoundEffect.FromFile("Assets/you_あ、見つかりましたぁ.wav").Play();
            }
        }

        private UniformStackContainer stackPanel;

        public override void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;
            Game.Input.KeyUp += Input_KeyUp;
            var rc = new RenderContainer(Game.GraphicsDevice)
            {
                RelativePosition = new(0, 0, 720, 576),
                Layout = Layout.Center
            };
            stackPanel = new UniformStackContainer()
            {
                RelativePosition = new(0, 0, 100, 100),
                Layout = Layout.Fill,
                Orientation = Orientation.Horizontal,
            };
            rc.AddChild(Game.AddObject(new Aquarium()
            {
                RelativePosition = new(0, 0, 1920, 1080)
            }));
            
            AddChild(rc);
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
                stackPanel.AddChild(Game.AddObject(new Aquarium()
                {
                    Layout = Layout.Fill,
                }));
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
