using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics;

namespace Water.Input
{
    public class InputManager
    {
        public event EventHandler<KeyEventArgs> KeyDown;

        public event EventHandler<KeyEventArgs> KeyUp;

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

            previousKbState = currentKbState;
            previousMouseState = currentMouseState;
        }

        public Point GetMousePositionRelativeTo(IContainer container)
        {
            return new(container.ActualPosition.X + currentMouseState.X, container.ActualPosition.Y + currentMouseState.Y);
        }

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
}
