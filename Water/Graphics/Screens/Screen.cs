using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Water.Graphics.Controls.UI;

namespace Water.Graphics.Screens
{
    public abstract class Screen : UIContextContainer
    {
        public virtual ScreenManager ScreenManager { get; set; }

        public virtual GameWindow Window { get; set; }

    }
}
