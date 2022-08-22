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
                    child.ActualPosition = new(offset + Margin, child.ActualPosition.Y + Margin, child.ActualPosition.Width, child.ActualPosition.Height);
                    offset += child.ActualPosition.X + Margin;
                }
                else if (Orientation == Orientation.Vertical)
                {
                    child.ActualPosition = new(child.ActualPosition.X + Margin, offset + Margin, child.ActualPosition.Width, child.ActualPosition.Height);
                    offset += child.ActualPosition.Y + Margin;
                }
                child.CalculateChildrenPositions();
            }
        }
    }
}
