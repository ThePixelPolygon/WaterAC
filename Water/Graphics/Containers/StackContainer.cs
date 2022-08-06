using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Graphics.Containers
{
    public class StackContainer : Container
    {
        public Orientation Orientation { get; set; }

        public override void CalculateChildrenPositions()
        {
            int offset = 0;
            if (Children.Count <= 0) return;

            foreach (var child in Children)
            {
                if (Orientation == Orientation.Horizontal)
                { 
                    child.ActualPosition = new(offset, child.RelativePosition.Y, child.RelativePosition.Width, child.RelativePosition.Height);
                    offset += child.RelativePosition.X;
                }
                else if (Orientation == Orientation.Vertical)
                {
                    child.ActualPosition = new(child.RelativePosition.X, offset, child.RelativePosition.Width, child.RelativePosition.Height);
                    offset += child.RelativePosition.Y;
                }
                child.CalculateChildrenPositions();
            }
        }
    }
}
