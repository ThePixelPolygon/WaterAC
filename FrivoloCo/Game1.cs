using FrivoloCo.Screens;
using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Water;

namespace FrivoloCo
{
    public class Game1 : WaterGame
    {
        public override string ProjectName /*=> "Ian Clary's First Day At FrivoloCo!";*/
        {
            get
            {
                return Random.Shared.Next(0, 6) switch
                {
                    5 => "Ian Clary's First Day At FrivoloCo! (trying to get over Brittani)",
                    _ => "Ian Clary's First Day At FrivoloCo!"
                };
            }
        }

        public Game1() : base()
        {
            GameScalingMode = GameScalingMode.MaintainAspectRatio;
            TargetWidth = 1920;
            TargetHeight = 1080;
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferWidth = 854;
            Graphics.PreferredBackBufferHeight = 480;
            Graphics.ApplyChanges();
            Screen.UpdateScreenSize(new(0, 0, 854, 480));
            Screen.ChangeScreen(new SplashScreen());
            Screen.AddScreen(new CreditOverlay());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}