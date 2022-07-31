using Microsoft.Xna.Framework;
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

        public Layout Layout { get; set; }
        public int Margin { get; set; }

        public void CalculateChildrenPositions();
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
        DockLeft,
        DockTop,
        DockRight,
        DockBottom,
        Fill,
        AspectRatioMaintainingFill
    }
}
