using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Containers;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens
{
    public class SplashScreen : Screen
    {
        private RenderContainer rc;
        private Sprite sp;

        public override void Deinitialize()
        {
            rc.Dispose();
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
            sp = new Sprite("Assets/splashscreen.png")
            {
                RelativePosition = new(0, 0, 1920, 1080),
                Layout = Water.Graphics.Layout.Fill
            };
            rc.AddChild(Game.AddObject(sp));
            AddChild(rc);

            Game.Audio.SwitchToTrack("Assets/Music/mainmenu.ogg", true);
        }

        private double counter = 0;
        private int stage = 0;

        public override void Update(GameTime gameTime)
        {
            counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (Game.Input.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Space))
                ScreenManager.ChangeScreen(new MenuScreen());

            if (stage == 0)
            {
                sp.Color = Color.White * (float)counter;
                if (counter >= 1)
                {
                    stage++;
                    counter = 0;
                }
            }
            else if (stage >= 1 && stage < 4)
            {
                if (counter >= 1)
                {
                    stage++;
                    counter = 0;
                }
            }
            else if (stage == 4)
            {
                sp.Color = Color.White * (float)(1 - counter);
                if (counter >= 1)
                {
                    stage++;
                    counter = 0;
                }
            }
            else ScreenManager.ChangeScreen(new MenuScreen());
        }
    }
}
