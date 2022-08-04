using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens.Menu
{
    public class MenuScreen : Screen
    {
        private RenderContainer rc;

        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Initialize()
        {
            rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Water.Graphics.Layout.Fill
            };
            var sp = new Sprite("Assets/FrivoloCoBackground.png")
            {
                RelativePosition = new(0, 0, 1920, 1080)
            };
            rc.AddChild(Game.AddObject(sp));
            AddChild(rc);

            var logo = new Sprite("Assets/logo.png")
            {
                RelativePosition = new(0, 0, 491, 308),
                Layout = Water.Graphics.Layout.AnchorTop,
                Margin = 10
            };
            AddChild(Game.AddObject(logo));

            //var sc = new StackContainer()
            //{
            //    RelativePosition = new(0, 0, 560, 630),
            //};
            var bt = new SpriteButton("Assets/indulge.png", "Assets/indulgeA.png", () => { throw new Exception("Test"); })
            {
                RelativePosition = new(0, 400, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            AddChild(Game.AddObject(bt));

            var bt2 = new SpriteButton("Assets/options.png", "Assets/optionsA.png", () => { throw new Exception("Test"); })
            {
                RelativePosition = new(0, 550, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            AddChild(Game.AddObject(bt2));

            var bt3 = new SpriteButton("Assets/exit.png", "Assets/exitA.png", () => { Environment.Exit(0); })
            {
                RelativePosition = new(0, 700, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            AddChild(Game.AddObject(bt3));
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
