using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;

namespace TestGame.Screens
{
    public class Aquarium : GameObject
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        private Box sprite;

        public override void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;
            Game.Input.KeyUp += Input_KeyUp;
            sprite = new Box()
            {
                RelativePosition = new(0, 0, 10, 10),
                Color = Color.CornflowerBlue,
                Layout = Layout.Fill,
                Margin = 2
            };
            AddChild(Game.AddObject(sprite));
        }

        private void Input_KeyUp(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.A)
            {
                sprite.Color = Color.CornflowerBlue;
            }
        }

        private void Input_KeyDown(object sender, Water.Input.KeyEventArgs e)
        {
            if (e.Key == Microsoft.Xna.Framework.Input.Keys.A)
            {
                sprite.Color = Color.ForestGreen;
            }
        }

        public override void Deinitialize()
        {
            Game.Input.KeyDown -= Input_KeyDown;
            Game.Input.KeyUp -= Input_KeyUp;
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
