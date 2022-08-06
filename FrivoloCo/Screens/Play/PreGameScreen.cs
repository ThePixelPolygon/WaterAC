using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Controls;
using Water.Graphics.Screens;

namespace FrivoloCo.Screens.Play
{
    public class PreGameScreen : Screen
    {
        private GameState state;

        public PreGameScreen(GameState state)
        {
            this.state = state;
        }

        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }
        private TextBlock tb;
        public override void Initialize()
        {
            tb = new TextBlock(new(0, 0, 0, 0), Game.Fonts.Get("Assets/Fonts/parisienne-regular.ttf", 90), "", Color.White)
            {
                Layout = Water.Graphics.Layout.Center,
                HorizontalTextAlignment = HorizontalTextAlignment.Center,
                VerticalTextAlignment = VerticalTextAlignment.Center,
                LineSpacing = 110
            };
            tb.Text = $"Day\n{state.Day}\nat FrivoloCo!\n\n${state.Money}";
            AddChild(Game.AddObject(tb));
            MediaPlayer.Stop();
            var fx = SoundEffect.FromFile("Assets/Music/gamestart.wav");
            fx.Play();
        }

        private double fadeIn = 0;
        private double jingleActiveLength = 2;
        private double jingleFadeLength = 3.5;
        public override void Update(GameTime gameTime)
        {
            if (fadeIn <= 1)
            {
                fadeIn += gameTime.ElapsedGameTime.TotalSeconds * 5;
                tb.Color = Color.White * (float)fadeIn;
                return;
            }

            jingleActiveLength -= gameTime.ElapsedGameTime.TotalSeconds;
            if (jingleActiveLength <= 0)
            {
                jingleFadeLength -= gameTime.ElapsedGameTime.TotalSeconds;
                var opacity = 1 - (1 / jingleFadeLength);
                tb.Color = Color.White * (float)opacity;
            }
            if (jingleFadeLength <= 0)
            {
                ScreenManager.ChangeScreen(new GameScreen(state));
            }
        }
    }
}
