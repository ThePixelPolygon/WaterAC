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
            base.Initialize();
        }

        private int counter = 10;

        public override void Update(GameTime gameTime)
        {
            counter--;
            if (counter < 0)
            {
                counter = 10;
                RelativePosition = new(RelativePosition.X, RelativePosition.Y - 1, RelativePosition.Width, RelativePosition.Height);

                if (RelativePosition.Y < (0 - RelativePosition.Height))
                    Game.RemoveObject(this);
            }
            base.Update(gameTime);
        }
    }
}
