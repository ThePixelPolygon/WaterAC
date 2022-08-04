using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens.Menu
{
    public class MenuScreen : Screen
    {
        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Initialize()
        {
            var tb = new TextBlock(new(0, 0, 500, 100), Game.Fonts.Get("Assets/Fonts/parisienne-regular.ttf", 50), "Welcome to FrivoloCo", Color.White)
            {
                Layout = Water.Graphics.Layout.Center
            };
            AddChild(Game.AddObject(tb));
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
