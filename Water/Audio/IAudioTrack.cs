using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Audio
{
    public interface IAudioTrack : IPlayable
    {
        public string FilePath { get; }

        public double Length { get; set; }

        public double Position { get; set; }

        public int Frequency { get; }

        public bool StreamLoaded { get; }

        public bool IsDisposed { get; }

        public bool HasPlayed { get; }

        public bool IsPlaying { get; }

        public bool IsPaused { get; }

        public bool IsStopped { get; }

        public bool IsLeftOver { get; }

        public bool AutoDispose { get; }

        public double Volume { get; set; }

        public double Time { get; }

        public void Play();

        public void Pause();

        public void Stop();
    }
}
