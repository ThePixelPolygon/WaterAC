using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Controls
{
    public class Box : GameObject
    {
        private Color color;
        private Texture2D sprite;
        public Box(Rectangle relativePosition, Color color)
        {
            RelativePosition = relativePosition;
            this.color = color;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(sprite, ActualPosition, color);
        }

        public override void Initialize()
        {
            sprite = new Texture2D(Game.GraphicsDevice, 1, 1);
            sprite.SetData(new[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
