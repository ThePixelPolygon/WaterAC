using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Screens;
using Water.Utils;

namespace Water.Graphics
{
    /// <summary>
    /// Main unit of the Water layout system
    /// </summary>
    public class Container : IContainer
    {
        /// <summary>
        /// Actual position of this container relative to the screen for use in the Draw method.
        /// Do not set this unless calculating children positions
        /// </summary>
        public Rectangle ActualPosition { get; set; }

        private Rectangle relativePosition;
        /// <summary>
        /// Position of this container relative to its parent. Some of the values may be ignored depending on the <see cref="Layout"/>
        /// </summary>
        public Rectangle RelativePosition
        {
            get => relativePosition;
            set
            {
                relativePosition = value;
                if (Parent is not null) Parent.CalculateChildrenPositions();
            }
        }
        /// <summary>
        /// Parent of this container. May be null if this is a root object (for example a <see cref="Screen"/>)
        /// </summary>
        public IContainer Parent { get; set; }
        /// <summary>
        /// Children of this container
        /// </summary>
        public List<IContainer> Children { get; private set; } = new();

        /// <summary>
        /// The way this container will be laid out by its parent. This might be ignored depending on the type of parent container.
        /// </summary>
        public Layout Layout { get; set; } = Layout.Manual;

        public Margins Margins { get; set; } = new(0);

        /// <summary>
        /// Adds a child to this container
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(IContainer child)
        {
            child.Parent = this;
            Children.Add(child);
            child.OnAddedToScreen();
            CalculateChildrenPositions();
        }
        /// <summary>
        /// Removes a child from this container
        /// </summary>
        /// <param name="child"></param>
        public virtual void RemoveChild(IContainer child)
        {
            child.Parent = null;
            Children.Remove(child);
            child.OnRemovedFromScreen();
            CalculateChildrenPositions();
        }

        public virtual void OnAddedToScreen()
        {

        }

        public virtual void OnRemovedFromScreen()
        {

        }

        /// <summary>
        /// Calculates the ActualPositions of this container's children. Override if you are a special kind of container
        /// </summary>
        public virtual void CalculateChildrenPositions()
        {
            foreach (var child in Children)
            {
                var parentPosition = child.Parent?.ActualPosition ?? RelativePosition; // can be null for the root object
                child.ActualPosition = child.Layout switch
                {
                    Layout.AnchorLeft => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Y + ((parentPosition.Height - child.RelativePosition.Height) / 2),
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorTopLeft => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Y + child.Margins.Top,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorTop => new
                    (
                        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                        parentPosition.Y + child.Margins.Top,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorTopRight => new
                    (
                        parentPosition.Right - child.RelativePosition.Width - child.Margins.Right,
                        parentPosition.Y + child.Margins.Top,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorRight => new
                    (
                        parentPosition.Right - child.RelativePosition.Width - child.Margins.Right,
                        parentPosition.Y + ((parentPosition.Height - child.RelativePosition.Height) / 2),
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorBottomRight => new
                    (
                        parentPosition.Right - child.RelativePosition.Width - child.Margins.Right,
                        parentPosition.Bottom - child.RelativePosition.Height - child.Margins.Bottom,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorBottom => new
                    (
                        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                        parentPosition.Bottom - child.RelativePosition.Height - child.Margins.Bottom,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.AnchorBottomLeft => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Bottom - child.RelativePosition.Height - child.Margins.Bottom,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.Center => new
                    (
                        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                        parentPosition.Y + ((parentPosition.Height - child.RelativePosition.Height) / 2),
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.HorizontalCenter => new
                    (
                        parentPosition.X + ((parentPosition.Width - child.RelativePosition.Width) / 2),
                        child.RelativePosition.Y,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    ),
                    Layout.Fill => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Y + child.Margins.Top,
                        parentPosition.Width - (child.Margins.Right * 2),
                        parentPosition.Height - (child.Margins.Bottom * 2)
                    ),
                    Layout.AspectRatioMaintainingFill => new
                    (
                        new Point
                        (
                            parentPosition.X + child.Margins.Left + child.Margins.Right,
                            parentPosition.Y + child.Margins.Left + child.Margins.Right
                        ),
                        InterfaceUtils.CalculateAspectRatioMaintainingFill(parentPosition, child.RelativePosition)
                    ),
                    Layout.DockLeft => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Y + child.Margins.Top,
                        child.RelativePosition.Width,
                        parentPosition.Height - (child.Margins.Bottom * 2)
                    ),
                    Layout.DockTop => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Y + child.Margins.Top,
                        parentPosition.Width - (child.Margins.Right * 2),
                        child.RelativePosition.Height
                    ),
                    Layout.DockRight => new
                    (
                        parentPosition.Right - child.RelativePosition.Width - child.Margins.Right,
                        parentPosition.Y + child.Margins.Top,
                        child.RelativePosition.Width,
                        parentPosition.Height - (child.Margins.Bottom * 2)
                    ),
                    Layout.DockBottom => new
                    (
                        parentPosition.X + child.Margins.Left,
                        parentPosition.Bottom - child.RelativePosition.Height - child.Margins.Top,
                        parentPosition.Width - (child.Margins.Right * 2),
                        child.RelativePosition.Height
                    ),
                    Layout.Manual or _ => new
                    (
                        parentPosition.X + child.RelativePosition.X,
                        parentPosition.Y + child.RelativePosition.Y,
                        child.RelativePosition.Width,
                        child.RelativePosition.Height
                    )

                };
                child.CalculateChildrenPositions();
            }
        } 

        /// <summary>
        /// Calls the Draw method of all of this container's children. Override if you need special children drawing behavior
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="graphicsDevice"></param>
        public virtual void DrawChildren(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {    
            foreach (var child in Children)
            {
                if (child is GameObject obj)
                {
                    obj.Draw(gameTime, spriteBatch, graphicsDevice);
                }
                else
                {
                    var childrenThatHaveChildrenThatAreGameObjects = child.Children.Where(x => x is GameObject);
                    foreach (var child2 in childrenThatHaveChildrenThatAreGameObjects)
                    {
                        if (child2 is GameObject obj2) obj2.Draw(gameTime, spriteBatch, graphicsDevice);
                    }
                }
                child.DrawChildren(gameTime, spriteBatch, graphicsDevice);
            }
        }

        
    }

    public struct Margins
    {
        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public Margins(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Margins(int horizontal, int vertical)
        {
            Left = horizontal;
            Top = vertical;
            Right = horizontal;
            Bottom = vertical;
        }

        public Margins(int value)
        {
            Left = value;
            Top = value;
            Right = value;
            Bottom = value;
        }
    }
}
