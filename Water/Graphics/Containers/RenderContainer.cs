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
                //var parentPosition = Children[0].RelativePosition;
                //Rectangle tempForDebugging = child.Layout switch
                //{
                //    Layout.AnchorLeft => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Y + ((parentPosition.Height - child.RelativePosition.Height) / 2),
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorTopLeft => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Y + child.Margin,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorTop => new
                //    (
                //        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                //        parentPosition.Y + child.Margin,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorTopRight => new
                //    (
                //        parentPosition.Right - child.RelativePosition.Width - child.Margin,
                //        parentPosition.Y + child.Margin,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorRight => new
                //    (
                //        parentPosition.Right - child.RelativePosition.Width - child.Margin,
                //        parentPosition.Y + ((parentPosition.Height - child.RelativePosition.Height) / 2),
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorBottomRight => new
                //    (
                //        parentPosition.Right - child.RelativePosition.Width - child.Margin,
                //        parentPosition.Bottom - child.RelativePosition.Height - child.Margin,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorBottom => new
                //    (
                //        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                //        parentPosition.Bottom - child.RelativePosition.Height - child.Margin,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.AnchorBottomLeft => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Bottom - child.RelativePosition.Height - child.Margin,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.Center => new
                //    (
                //        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                //        parentPosition.Y + ((parentPosition.Height - child.RelativePosition.Height) / 2),
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    ),
                //    Layout.Fill => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Y + child.Margin,
                //        parentPosition.Width - (child.Margin * 2),
                //        parentPosition.Height - (child.Margin * 2)
                //    ),
                //    Layout.AspectRatioMaintainingFill => new
                //    (
                //        new Point
                //        (
                //            parentPosition.X + child.Margin,
                //            parentPosition.Y + child.Margin
                //        ),
                //        CalculateAspectRatioMaintainingFill(parentPosition, child.RelativePosition)
                //    ),
                //    Layout.DockLeft => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Y + child.Margin,
                //        child.RelativePosition.Width,
                //        parentPosition.Height - (child.Margin * 2)
                //    ),
                //    Layout.DockTop => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Y + child.Margin,
                //        parentPosition.Width - (child.Margin * 2),
                //        child.RelativePosition.Height
                //    ),
                //    Layout.DockRight => new
                //    (
                //        parentPosition.Right - child.RelativePosition.Width - child.Margin,
                //        parentPosition.Y + child.Margin,
                //        child.RelativePosition.Width,
                //        parentPosition.Height - (child.Margin * 2)
                //    ),
                //    Layout.DockBottom => new
                //    (
                //        parentPosition.X + child.Margin,
                //        parentPosition.Bottom - child.RelativePosition.Height - child.Margin,
                //        parentPosition.Width - (child.Margin * 2),
                //        child.RelativePosition.Height
                //    ),
                //    Layout.Manual or _ => new
                //    (
                //        parentPosition.X + child.RelativePosition.X,
                //        parentPosition.Y + child.RelativePosition.Y,
                //        child.RelativePosition.Width,
                //        child.RelativePosition.Height
                //    )

                //};
                //child.ActualPosition = tempForDebugging;
                child.CalculateChildrenPositions();
            }
        }

        private Point CalculateAspectRatioMaintainingFill(Rectangle parentPosition, Rectangle childPosition)
        {
            float parentAspectRatio = parentPosition.Width / parentPosition.Height;
            float childAspectRatio = childPosition.Width / childPosition.Height;

            float scalingFactor;
            if (parentAspectRatio > childAspectRatio) scalingFactor = parentPosition.Width / childPosition.Width;
            else scalingFactor = parentPosition.Height / childPosition.Height;

            return new((int)(childPosition.Width * scalingFactor), (int)(childPosition.Height * scalingFactor));
        }
    }
}
