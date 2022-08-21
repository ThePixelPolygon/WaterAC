using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Configuration
{
    public class ConfigFile
    {
        public float MasterVolume { get; set; } = 1;

        public float MusicVolume { get; set; } = 0.5f;

        public float EffectVolume { get; set; } = 1;
    }
}
