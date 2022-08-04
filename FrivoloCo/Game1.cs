using FrivoloCo.Screens.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Water;

namespace FrivoloCo
{
    public class Game1 : WaterGame
    {
        public override string ProjectName => "FrivoloCo: The Game";

        public Game1() : base()
        {

        }

        protected override void Initialize()
        {
            Screen.ChangeScreen(new MenuScreen());
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