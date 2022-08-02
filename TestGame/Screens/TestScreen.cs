using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private StackContainer stackPanel;

        public override void Initialize()
        {
            Game.Input.KeyDown += Input_KeyDown;
            Game.Input.KeyUp += Input_KeyUp;
            stackPanel = new StackContainer()
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
