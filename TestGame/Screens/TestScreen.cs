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

        private Sprite sprite;

        public override void Initialize()
        {
            sprite = new Sprite(new(10, 10, 1280, 720), "Assets/Chiruuu.png")
            {
                Layout = Layout.Center
            };
            AddChild(Game.AddObject(sprite));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        private int counter = 500;

        public override void Update(GameTime gameTime)
        {
            counter--;
            if (counter < 0)
            {
                counter = 100;
                var bubble = new Bubble();
                sprite.AddChild(bubble);
                Game.AddObject(bubble);
            }
        }
    }
}
