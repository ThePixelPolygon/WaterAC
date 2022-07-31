using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;

namespace Water.Graphics.Containers
{
    public class StackContainer : Container
    {
        public Orientation Orientation { get; set; }

        public override void CalculateChildrenPositions()
        {
            int offset;
            if (Children.Count <= 0) return;

            if (Orientation == Orientation.Horizontal) offset = ActualPosition.Width / Children.Count;
            else offset = ActualPosition.Height / Children.Count;

            int i = 0;
            foreach (var child in Children)
            {
                var newPosition = offset * i;
                if (Orientation == Orientation.Horizontal) child.ActualPosition = new(ActualPosition.X + newPosition, ActualPosition.Y, offset, ActualPosition.Height);
                else child.ActualPosition = new(ActualPosition.X, ActualPosition.Y + newPosition, ActualPosition.Width, offset);
                child.CalculateChildrenPositions();
                i++;
            }
        }
    }
    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}
