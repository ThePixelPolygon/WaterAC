using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;
using Water.Utils;

namespace FrivoloCo.Screens.Menu
{
    public class OptionsScreen : Screen
    {
        public override void Deinitialize()
        {
            rc.Dispose();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

        }

        private RenderContainer rc;

        public override void Initialize()
        {
            rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Water.Graphics.Layout.Fill
            };

            var co = new Container()
            {
                RelativePosition = new(0, 0, 1920, 1080)
            };
            rc.AddChild(co);

            var sp = new Sprite("Assets/FrivoloCoBackground.png")
            {
                RelativePosition = new(0, 0, 1920, 1080)
            };
            co.AddChild(Game.AddObject(sp));
            AddChild(rc);

            var tb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 40), "", Color.Black)
            {
                Layout = Water.Graphics.Layout.Fill,
                HorizontalTextAlignment = HorizontalTextAlignment.Left,
                VerticalTextAlignment = VerticalTextAlignment.Top,
                Margins = new(10)
            };
            tb.Text = "since we don't have a proper options UI yet, the config file should have been opened with your text editor.\n" +
                "just edit that and your changes will be saved when you hit the back button :)";
            var box = new Box()
            {
                Layout = Water.Graphics.Layout.Fill,
                Margins = new(10),
                Color = Color.White * 0.5f
            };
            co.AddChild(Game.AddObject(box));
            box.AddChild(Game.AddObject(tb));

            var button = new SpriteButton("Assets/back.png", "Assets/backA.png", () => 
            {
                Game.MainGame.LoadConfigAsync().GetAwaiter().GetResult(); // TODO: make this not cursed
                ScreenManager.ChangeScreen(new MenuScreen()); 
            })
            {
                Layout = Water.Graphics.Layout.AnchorBottomLeft,
                RelativePosition = new(0, 0, 250, 100),
                Margins = new(10)
            };
            co.AddChild(Game.AddObject(button));

            InterfaceUtils.OpenURL(Path.GetFullPath("Data/engineconfig.json"));
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
