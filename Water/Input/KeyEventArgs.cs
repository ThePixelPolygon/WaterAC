using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Input
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; init; }

        public KeyEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
