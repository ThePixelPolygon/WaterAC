using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Screens;
using Water;

namespace TestGame
{
    public class Game1 : WaterGame
    {
        public override string ProjectName => "nyaa";

        public Game1() : base()
        {
        }

        protected override void Initialize()
        {

            Screen.InsertScreen(0, new TestScreen());
            base.Initialize();
        }

        protected override void LoadContent()
        {
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