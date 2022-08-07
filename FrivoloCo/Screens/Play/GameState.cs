using FrivoloCo.Screens.Play.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrivoloCo.Screens.Play
{
    public class GameState
    {
        public double TimeLeft { get; set; } = 120000; // 2 minutes in milliseconds

        public double TableTopOffset { get; set; } = 448; // Pixels to top edge of visible table

        public double TimeDelayBetweenCustomersMax { get; set; } = 5000; // in milliseconds

        public double TimeDelayBetweenCustomers { get; set; } = 5000; // in milliseconds

        public int Strikes { get; set; } = 0;

        public Item CurrentlyDraggedItem { get; set; } = null;

        public bool Paused { get; set; } = false;
    }
}
