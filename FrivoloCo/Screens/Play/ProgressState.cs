using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrivoloCo.Screens.Play
{
    public class ProgressState
    {
        public DateTime StartedPlaying { get; set; } = DateTime.Now;

        public int Day { get; set; } = 1;

        public decimal Money { get; set; } = 1;

        public int SongPlayed { get; set; } = 1;

        public int TotalStrikes { get; set; } = 0;

        public int CustomersServed { get; set; } = 0;
    }
}
