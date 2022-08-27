using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Audio
{
    public interface IEffectTrack : IPlayable
    {
        public bool IsPlaying { get; }
        public bool IsPaused { get; }

        public bool IsStopped { get; }
        public bool HasStopped { get; }

        public bool IsLeftOver { get; }
    }
}
