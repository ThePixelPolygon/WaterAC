using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;
using Water.Graphics.Containers;

namespace Water.Input
{
    public class InputManager
    {
        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;

        public event EventHandler<MousePressEventArgs> PrimaryMouseButtonDown;
        public event EventHandler<MousePressEventArgs> SecondaryMouseButtonDown;

        public event EventHandler<MousePressEventArgs> PrimaryMouseButtonUp;
        public event EventHandler<MousePressEventArgs> SecondaryMouseButtonUp;

        private readonly GameObjectManager game;

        public InputManager(GameObjectManager game)
        {
            this.game = game;
        }

        private KeyboardState previousKbState;
        private KeyboardState currentKbState;

        private MouseState previousMouseState;
        private MouseState currentMouseState;

        public void Update(GameTime gameTime)
        {
            currentKbState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            var pressedKeys = currentKbState.GetPressedKeys();
            var previousPressedKeys = previousKbState.GetPressedKeys();
            foreach (var key in pressedKeys.Where(x => !previousPressedKeys.Contains(x)))
            {
                KeyDown?.Invoke(this, new(key));
            }
            foreach (var key in previousPressedKeys.Where(x => !pressedKeys.Contains(x)))
            {
                KeyUp?.Invoke(this, new(key));
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && game.MainGame.IsActive) PrimaryMouseButtonDown?.Invoke(this, new());
            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton != ButtonState.Released && game.MainGame.IsActive) PrimaryMouseButtonUp?.Invoke(this, new());

            if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed && game.MainGame.IsActive) SecondaryMouseButtonDown?.Invoke(this, new());
            if (currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton != ButtonState.Released && game.MainGame.IsActive) SecondaryMouseButtonUp?.Invoke(this, new());

            previousKbState = currentKbState;
            previousMouseState = currentMouseState;
        }

        public Point GetMousePositionRelativeTo(IContainer container)
        {
            float scaleFactorX = 1;
            float scaleFactorY = 1;

            int offsetX = 0;
            int offsetY = 0;

            var x = container;
            RenderContainer parentRenderContainer = null;
            if (x is RenderContainer y) parentRenderContainer = y;
            else if (x.Parent is not null)
            {
                while (x.Parent is not null)
                {
                    x = x.Parent;
                    if (x is RenderContainer rc)
                    {
                        parentRenderContainer = rc;
                        break;
                    }
                }
            }

            if (parentRenderContainer is not null)
            {
                
                var rcVirtualized = parentRenderContainer.Children[0].RelativePosition;

                var scaleX = (float)rcVirtualized.Width / parentRenderContainer.ActualPosition.Width /*/ rcVirtualized.Width*/;
                var scaleY = (float)rcVirtualized.Height / parentRenderContainer.ActualPosition.Height /*/ rcVirtualized.Height*/;

                var aa = ((container.RelativePosition.X + currentMouseState.X) * scaleX);
                var a = parentRenderContainer.ActualPosition.X + (int)Math.Round(aa);
                var b = parentRenderContainer.ActualPosition.Y + (int)Math.Round((float)( container.RelativePosition.Y + currentMouseState.Y) * scaleY);
                return new(a,
                    b);
            }
            //return new(100, 100);
            //else
                return new((int)Math.Round((container.RelativePosition.X + currentMouseState.X + offsetX) * (scaleFactorX * 1)), (int)Math.Round((container.RelativePosition.Y + currentMouseState.Y + offsetY) * (scaleFactorY * 1)));
        }

        public bool IsMouseWithin(IContainer container)
        {
            var x = container;
            RenderContainer parentRenderContainer = null;
            if (x.Parent is not null)
            {
                while (x.Parent is not null)
                {
                    x = x.Parent;
                    if (x is RenderContainer rc)
                    {
                        parentRenderContainer = rc;
                        break;
                    }
                }
            }
            

            if (parentRenderContainer is not null)
            {
                var relativeMousePos = GetMousePositionRelativeTo(parentRenderContainer);
                return relativeMousePos.X >= container.ActualPosition.X && relativeMousePos.X <= (container.ActualPosition.X + container.ActualPosition.Width) &&
                   relativeMousePos.Y >= container.ActualPosition.Y && relativeMousePos.Y <= (container.ActualPosition.Y + container.ActualPosition.Height);
            }
            else
                return currentMouseState.X >= container.ActualPosition.X && currentMouseState.X <= (container.ActualPosition.X + container.ActualPosition.Width) &&
                   currentMouseState.Y >= container.ActualPosition.Y && currentMouseState.Y <= (container.ActualPosition.Y + container.ActualPosition.Height);
        }

        public bool IsPrimaryMouseButtonHeld() => currentMouseState.LeftButton == ButtonState.Pressed;

        public bool IsKeyHeld(Keys key) => currentKbState.GetPressedKeys().Contains(key);
    }

    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; init; }

        public KeyEventArgs(Keys key)
        {
            Key = key;
        }
    }

    public class MousePressEventArgs : EventArgs
    {
        public MousePressEventArgs()
        {
            // to be used in the future
        }
    }
}
