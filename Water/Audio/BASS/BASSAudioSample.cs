using ManagedBass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Audio.BASS
{
    public class BASSAudioSample : IDisposable
    {
        public int Id { get; }
        public bool HasPlayed { get; set; }
        public bool OnlyCanPlayOnce { get; }
        public bool IsDisposed { get; private set; }

        public BASSAudioSample(string path, bool onlyCanPlayOnce = false)
        {
            OnlyCanPlayOnce = onlyCanPlayOnce;
            Id = Load(path, 10);
        }

        private int Load(string path, int concurrentPlaybacks)
        {
            return Bass.SampleLoad(path, 0, 0, concurrentPlaybacks, BassFlags.Default | BassFlags.SampleOverrideLongestPlaying);
        }

        public void Dispose()
        {
            Bass.SampleFree(Id);
            IsDisposed = true;
        }

    }
}
