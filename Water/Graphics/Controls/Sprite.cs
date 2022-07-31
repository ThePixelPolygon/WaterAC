using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Controls
{
    public class Sprite : GameObject
    {
        private Texture2D sprite;
        private readonly string path;
        public Sprite(Rectangle relativePosition, string path)
        {
            RelativePosition = relativePosition;
            this.path = path;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(sprite, ActualPosition, Color.White);
        }

        public override void Initialize()
        {
            sprite = Game.Textures.Get(path);
        }

        public override void Update(GameTime gameTime)
        {
 
        }
    }
}
