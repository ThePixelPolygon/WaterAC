using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Water.Graphics
{
    public abstract class GameObject : Container, IDrawableThing
    {
        public GameObjectManager Game { get; set; }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
}
