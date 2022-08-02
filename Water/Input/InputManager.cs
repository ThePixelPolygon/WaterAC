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

        private KeyboardState previousState;

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            var pressedKeys = state.GetPressedKeys();
            var previousPressedKeys = previousState.GetPressedKeys();
            foreach (var key in pressedKeys.Where(x => !previousPressedKeys.Contains(x)))
            {
                KeyDown?.Invoke(this, new(key));
            }
            foreach (var key in previousPressedKeys.Where(x => !pressedKeys.Contains(x)))
            {
                KeyUp?.Invoke(this, new(key));
            }

            previousState = state;
        }

        public bool IsKeyHeld(Keys key) => Keyboard.GetState().GetPressedKeys().Contains(key);
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
