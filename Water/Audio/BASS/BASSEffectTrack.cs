using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;

namespace Water.Audio.BASS
{
    public class BASSEffectTrack : IEffectTrack
    {
        private BASSAudioSample sample;

        private int id;

        public bool HasPlayed { get; private set; }

        public bool IsPlaying { get; }
        public bool IsPaused { get; }

        public bool IsStopped => Bass.ChannelIsActive(id) == PlaybackState.Stopped;
        public bool HasStopped { get; private set; }

        public bool IsLeftOver => HasPlayed && IsStopped;
        public bool AutoDispose { get; }

        public double Volume
        {
            get => Bass.ChannelGetAttribute(id, ChannelAttribute.Volume);
            set => Bass.ChannelSetAttribute(id, ChannelAttribute.Volume, value);
        }

        public double Time { get; }

        public BASSEffectTrack(BASSAudioSample sample, float rate = 1f, float pan = 0f)
        {
            this.sample = sample;

            if (sample.IsDisposed)
                throw new Exception("Kyaa! Sample already disposed!");

            id = Bass.SampleGetChannel(sample.Id);

            if (rate != 1f)
            {
                var frequency = Bass.ChannelGetInfo(id).Frequency;
                Bass.ChannelSetAttribute(id, ChannelAttribute.Frequency, frequency * rate);
            }

            if (pan != 0f)
                Bass.ChannelSetAttribute(id, ChannelAttribute.Pan, pan);
        }

        public void Dispose()
        {
            sample.Dispose();
        }

        public void Pause()
        {
            Bass.ChannelPause(id);
        }

        public void Play()
        {
            if (sample.HasPlayed && sample.OnlyCanPlayOnce)
                throw new Exception("Kyaa! >.< you can't play a sample more than once");
            if (sample.IsDisposed)
                throw new Exception("Kyaa! >.< you can't play a sample that's disposed!");

            Bass.ChannelPlay(id);
            sample.HasPlayed = true;
            HasPlayed = true;
        }

        public void Stop()
        {
            if (HasStopped)
                throw new Exception("Kyaa! Already stopped!");
            if (sample.IsDisposed)
                throw new Exception("Kyaa! Already disposed!");

            Bass.ChannelStop(id);
            HasStopped = true;
        }
    }
}
