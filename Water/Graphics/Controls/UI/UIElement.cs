using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Controls.UI
{
    public class UIElement : GameObject
    {
        public bool Index { get; set; }

        public bool IsFocused { get; set; }

        private UIContextContainer uiContext;

        public override void OnAddedToScreen()
        {
            IContainer container = Parent;

            while (true)
            {
                if (container is UIContextContainer x)
                {
                    uiContext = container as UIContextContainer;
                    break;
                }
                else if (container.Parent is null)
                    throw new Exception("Kyaa! There are no screens or other UI Context containers up the hierarchy from this control >.<");
                else
                    container = container.Parent; 
            }

            base.OnAddedToScreen();
        }

        public override void OnRemovedFromScreen()
        {
            base.OnRemovedFromScreen();
        }

        public override void Deinitialize()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Initialize()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
