using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrivoloCo.Screens.Play.Items
{
    public class Tray : Item
    {
        public override string Name => "Tray";

        public Tray(bool isBeingDragged, GameState state) : base("Assets/Gameplay/tray.png", isBeingDragged, state)
        {
            RelativePosition = new(0, 0, 437, 72);
        }
    }
}
