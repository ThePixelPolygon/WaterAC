using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Screens
{
    public abstract class Screen : IDrawableThing
    {
        public virtual GameObjectManager Game { get; set; }

        public virtual GameObjectScreen ScreenManager { get; set; }

        public virtual GameWindow Window { get; set; }

        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
}
