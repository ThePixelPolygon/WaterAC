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
        public TestScreen()
        {
            
        }

        public override void Initialize()
        {
            var stackPanel = new StackContainer()
            {
                RelativePosition = new(0, 0, 100, 100),
                Layout = Layout.Fill,
                Orientation = Orientation.Horizontal,
            };
            AddChild(stackPanel);
            stackPanel.AddChild(Game.AddObject(new Aquarium()
            {
                Layout = Layout.Fill,
            }));
            stackPanel.AddChild(Game.AddObject(new Aquarium()
            {
                Layout = Layout.Fill,
            }));  
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }


        public override void Update(GameTime gameTime)
        {

        }
    }
}
