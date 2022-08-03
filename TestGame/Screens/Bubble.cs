using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Controls;

namespace TestGame.Screens
{
    public class Bubble : Box
    {
        public Bubble() : base()
        {
            Color = Color.White;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            base.Draw(gameTime, spriteBatch, graphicsDevice);
        }

        public override void Initialize()
        {
            RelativePosition = new(Random.Shared.Next(0, Parent.ActualPosition.Width), Random.Shared.Next(0, Parent.ActualPosition.Height), 10, 10);
            Game.Input.PrimaryMouseButtonDown += Input_PrimaryMouseButtonDown;
            base.Initialize();
        }

        private void Input_PrimaryMouseButtonDown(object sender, Water.Input.MousePressEventArgs e)
        {
            if (Game.Input.IsMouseWithin(this)) Game.RemoveObject(this);
        }

        public override void Deinitialize()
        {
            Game.Input.PrimaryMouseButtonDown -= Input_PrimaryMouseButtonDown;
            base.Deinitialize();
        }

        private int counter = 10;

        public override void Update(GameTime gameTime)
        {
            if (Game.Input.IsMouseWithin(this)) Color = Color.Red;
            else Color = Color.White;
            counter--;
            if (counter < 0)
            {
                counter = 10;
                RelativePosition = new(RelativePosition.X, (int)Math.Round(RelativePosition.Y - 1 * gameTime.ElapsedGameTime.TotalMilliseconds), RelativePosition.Width, RelativePosition.Height);

                if (RelativePosition.Y < (0 - RelativePosition.Height))
                    Game.RemoveObject(this);
            }
            base.Update(gameTime);
        }
    }
}
