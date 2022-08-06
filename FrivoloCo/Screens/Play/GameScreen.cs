using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens.Play
{
    public class GameScreen : Screen
    {
        public GameScreen(GameState state)
        {

        }

        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        GameObject b;
        public override void Initialize()
        {
            MediaPlayer.Play(Song.FromUri("gameplay-1", new("Assets/Music/gameplay-1.ogg", UriKind.Relative)));

            var rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Water.Graphics.Layout.Fill
            };
            var sp = new Sprite("Assets/Gameplay/gameplayday.png")
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Layout.Fill
            };
            var co = new Container()
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Layout.Center
            };
            co.AddChild(Game.AddObject(sp));

            b = new SpriteButton("Assets/back.png", "Assets/backA.png", () => { ScreenManager.ChangeScreen(new MenuScreen()); })
            {
                Layout = Water.Graphics.Layout.Center,
                RelativePosition = new(0, 0, 250, 100)
            };
            co.AddChild(Game.AddObject(b));

            rc.AddChild(co);

            AddChild(rc);
        }

        public override void Update(GameTime gameTime)
        {
            //var pos = Game.Input.GetMousePositionRelativeTo(this);
            //if (pos.Y >= 540)
            //{

            //}
            //b.RelativePosition = new(pos.X, pos.Y, 250, 100);
        }
    }
}
