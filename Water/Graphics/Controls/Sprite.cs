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
        public Color Color { get; set; } = Color.White;

        public string Path
        {
            get => path;
            set
            {
                path = value;
                UpdateSprite();
            }
        }

        private Texture2D sprite;
        private string path;
        public Sprite(string path)
        {
            this.path = path;
        }

        private void UpdateSprite()
        {
            sprite = Game.Textures.Get(path);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(sprite, ActualPosition, Color);
        }

        public override void Initialize() => UpdateSprite();

        public override void Deinitialize()
        {
       
        }

        public override void Update(GameTime gameTime)
        {
 
        }
    }
}
