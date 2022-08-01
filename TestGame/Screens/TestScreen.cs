using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace TestGame.Screens
{
    public class TestScreen : Screen
    {
        public TestScreen()
        {
            
        }

        public override void Initialize()
        {
            var box = new Sprite(new(10, 10, 1280, 720), "Assets/Sylux6.png")
            {
                Layout = Layout.Center
            };
            AddChild(Game.AddObject(box));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
