using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
    public class MenuScreen : Screen
    {
        private RenderContainer rc;
        private arcadeShim ArcadeShim = arcadeShim.getInstance();
        public override void Deinitialize()
        {
            rc.Dispose();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        private Sprite sp;
        private Sprite logo;
        private Sprite coin;
        private Sprite bt;
        // private Sprite bt2;
        // private Sprite bt3;
        // private Sprite bt4;
        private TextBlock tx;

        private Container co;
        
        public override void Initialize()
        {
            if (Game.Audio.Tracks.Find(x => x.FilePath == "Assets/Music/mainmenu.ogg") is null)
                Game.Audio.SwitchToTrack("Assets/Music/mainmenu.ogg", true);

            rc = new RenderContainer(Game.GraphicsDevice)
            {
                Layout = Water.Graphics.Layout.Fill
            };

            co = new Container()
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

            logo = new Sprite("Assets/frivologo.png")
            {
                RelativePosition = new(0, 0, 393, 225),
                Layout = Water.Graphics.Layout.AnchorTop,
                Margins = new(50)
            };

            co.AddChild(Game.AddObject(logo));

            coin = new Sprite("Assets/insertCoin.png")
            {
                RelativePosition = new Rectangle(0, 500, 964, 200),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };
            
            co.AddChild(Game.AddObject(coin));
            
            bt = new SpriteButton("Assets/indulge.png", "Assets/indulgeA.png", () => { ScreenManager.ChangeScreen(new GamemodeScreen()); })
            {
                RelativePosition = new(0, 300, 250, 100),
                Layout = Water.Graphics.Layout.HorizontalCenter
            };

            tx = new TextBlock(new(0, 0, 500, 18), Game.Fonts.Get("Assets/IBMPLEXSANS-MEDIUM.TTF", 20), "FCO:U:A:A:20230206", Color.White)
            {
                Layout = Water.Graphics.Layout.AnchorBottomLeft,
                HorizontalTextAlignment = HorizontalTextAlignment.Left,
                VerticalTextAlignment = VerticalTextAlignment.Bottom
            };
            co.AddChild(Game.AddObject(tx));
        }

        private double counter = 0;
        public override void Update(GameTime gameTime)
        {
            counter += gameTime.ElapsedGameTime.TotalSeconds;
            int currentCredits = ArcadeShim.Credits;

            if (currentCredits > 0)
            {
                
            }
            
            sp.Color = logo.Color = coin.Color = bt.Color = tx.Color = Color.White * (float)counter;
            
            
        }
    }
}
