using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Screens;

namespace Water.Graphics.Containers
{
    public class RenderContainer : Container
    {
        private RenderTarget2D renderTarget;
        private GraphicsDevice graphicsDevice;

        public RenderContainer(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public override void AddChild(IContainer child)
        {
            if (Children.Count == 0)
            {
                renderTarget = new RenderTarget2D
                (
                    graphicsDevice,
                    child.RelativePosition.Width,
                    child.RelativePosition.Height,
                    false,
                    graphicsDevice.PresentationParameters.BackBufferFormat,
                    DepthFormat.Depth24
                );
            }
            else throw new InvalidOperationException("RenderContainers can only have one child.");

            base.AddChild(child);
        }

        public override void DrawChildren(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.End();
            
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new() { DepthBufferEnable = true };

            spriteBatch.Begin();
            graphicsDevice.Clear(Color.Black);
            base.DrawChildren(gameTime, spriteBatch, graphicsDevice);
            spriteBatch.End();

            spriteBatch.Begin();
            graphicsDevice.SetRenderTarget(null);

            spriteBatch.Draw(renderTarget, ActualPosition, Color.White);
        }

        public override void CalculateChildrenPositions()
        {
            foreach (var child in Children)
            {
                child.ActualPosition = Children[0].RelativePosition;
                child.CalculateChildrenPositions();
            }
        }

    }

    public enum ScaleMode
    {
        UniformToFill,
        Fill
    }
}
