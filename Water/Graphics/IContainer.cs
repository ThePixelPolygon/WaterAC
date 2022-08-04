using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics
{
    public interface IContainer
    {
        public Rectangle ActualPosition { get; set; }
        public Rectangle RelativePosition { get; set; }

        public IContainer Parent { get; set; }
        public List<IContainer> Children { get; }

        public Layout Layout { get; set; }
        public int Margin { get; set; }

        public void AddChild(IContainer child);
        public void RemoveChild(IContainer child);

        public void CalculateChildrenPositions();

        public void DrawChildren(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
    public enum Layout
    {
        Manual,
        AnchorLeft,
        AnchorTopLeft,
        AnchorTop,
        AnchorTopRight,
        AnchorRight,
        AnchorBottomRight,
        AnchorBottom,
        AnchorBottomLeft,
        Center,
        HorizontalCenter,
        VerticalCenter,
        DockLeft,
        DockTop,
        DockRight,
        DockBottom,
        Fill,
        AspectRatioMaintainingFill
    }
}
