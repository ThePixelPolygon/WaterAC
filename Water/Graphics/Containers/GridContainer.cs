using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;

namespace Water.Graphics.Containers
{
    public class GridContainer : Container
    {
        public new IContainer[,] Children { get; set; }

        public void AddChild(IContainer child, int x, int y)
        {
            child.Parent = this;
            Children[x, y] = child;
        }

        public override void CalculateChildrenPositions()
        {
            var itemWidth = ActualPosition.Width / Children.GetLength(0);
            var itemHeight = ActualPosition.Height / Children.GetLength(1);

            for (int x = 0; x < Children.GetLength(0); x++)
            {
                for (int y = 0; y < Children.GetLength(1); y++)
                {
                    var child = Children[x, y];
                    var newX = itemWidth * x;
                    var newY = itemHeight * y;
                    child.ActualPosition = new(ActualPosition.X + newX, ActualPosition.Y + newY, itemWidth, itemHeight);
                    child.CalculateChildrenPositions();
                }
            }
        }
    }
}