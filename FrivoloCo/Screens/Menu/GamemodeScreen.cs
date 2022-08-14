using FrivoloCo.Screens.Play;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens.Menu
{
    public class GamemodeScreen : Screen
    {
        private RenderContainer rc;

        public override void Deinitialize()
        {
            rc.Dispose();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

        }

        private Sprite sp;
        private Sprite bt;
        private Sprite bt2;
        private Sprite bt3;

        private bool beginGame = false;

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

            sp = new Sprite("Assets/FrivoloCoBackground.png")
            {
                RelativePosition = new(0, 0, 1920, 1080)
            };
            co.AddChild(Game.AddObject(sp));
            AddChild(rc);

            bt = new SpriteButton("Assets/endless.png", "Assets/endlessA.png", () => { beginGame = true; })
            {
                RelativePosition = new(0, 300, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            co.AddChild(Game.AddObject(bt));

            bt2 = new SpriteButton("Assets/tutorial.png", "Assets/tutorialA.png", () => { ScreenManager.ChangeScreen(new OptionsScreen()); })
            {
                RelativePosition = new(0, 450, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            co.AddChild(Game.AddObject(bt2));

            bt3 = new SpriteButton("Assets/back.png", "Assets/backA.png", () => { ScreenManager.ChangeScreen(new MenuScreen()); })
            {
                RelativePosition = new(0, 600, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            co.AddChild(Game.AddObject(bt3));
        }

        private double counter = 1;
        public override void Update(GameTime gameTime)
        {
            if (beginGame) counter -= gameTime.ElapsedGameTime.TotalSeconds * 5;
            sp.Color = bt.Color = bt2.Color = bt3.Color = Color.White * (float)counter;

            if (counter <= 0)
            {
                ScreenManager.ChangeScreen(new PreGameScreen(new ProgressState()));
                beginGame = false;
                return;
            }
        }
    }
}
