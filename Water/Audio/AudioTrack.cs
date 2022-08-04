using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;

namespace Water.Audio
{
    public class AudioTrack : IPlayable
    {
        public string FilePath { get; }

        public int Stream { get; private set; }

        private double length = -1;
        public double Length
        {
            get => length;
            set
            {
                if (length != -1)
                    throw new Exception("Kyaa! Can't set length manually");

                length = value;
            }
        }

        public double Position
        {
            get
            {
                return Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetPosition(Stream) * 1000);
            }
        }

        public int Frequency { get; private set; }

        public bool StreamLoaded => Stream != 0;

        public bool IsDisposed { get; private set; }

        public bool HasPlayed { get; private set; }

        public bool IsPlaying => Bass.ChannelIsActive(Stream) == PlaybackState.Playing;

        public bool IsPaused => Bass.ChannelIsActive(Stream) == PlaybackState.Paused;

        public bool IsStopped => Bass.ChannelIsActive(Stream) == PlaybackState.Stopped;

        public bool IsLeftOver => HasPlayed && IsStopped;

        public bool AutoDispose { get; }

        public double Volume
        {
            get => Bass.ChannelGetAttribute(Stream, ChannelAttribute.Volume) * 100;
            set => Bass.ChannelSetAttribute(Stream, ChannelAttribute.Volume, (float)(value / 100f));
        }

        public double Time => Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetPosition((Stream)) * 1000);

        private AudioManager audioManager;
        public AudioTrack(AudioManager audioManager, string path, bool autoDispose = true)
        {
            this.audioManager = audioManager;
            FilePath = path;
            AutoDispose = autoDispose;
            Volume = 1f;
            Stream = Bass.CreateStream(path/*, Flags: BassFlags.Decode | BassFlags.Prescan*/);

            AfterLoad();
        }

        public void Dispose()
        {
            Bass.StreamFree(Stream);
            Stream = 0;
            IsDisposed = true;
        }

        public void Pause()
        {
            CheckIfDisposed();

            if (!IsPlaying || IsPaused)
                throw new Exception("Kyaa! Can't pause if it's already paused or isn't playing");

            Bass.ChannelPause(Stream);
        }

        public void Play()
        {
            CheckIfDisposed();

            if (!IsPlaying || IsPaused)
                throw new Exception("Kyaa! Tried to play a track that's already playing");

            var previous = Time;

            var result = Bass.ChannelPlay(Stream);
            if (!result)
            {
                throw new Exception($"Kyaa! Playing audio didn't work: {Bass.LastError}");
            }
            HasPlayed = true;

            if (Time < previous)
            {
                // TODO: implement
            }
        }

        public void Stop()
        {
            CheckIfDisposed();

            Bass.ChannelStop(Stream);

            if (AutoDispose) Dispose();
        }

        public void Restart()
        {
            CheckIfDisposed();

            if (IsPlaying) Pause();

            Seek(0);
            Play();
        }

        public void Seek(double position)
        {
            CheckIfDisposed();

            if (position > Length || position < -1)
                throw new Exception("Kyaa! Invalid position!");

            var previous = Time;
            Bass.ChannelSetPosition(Stream, Bass.ChannelSeconds2Bytes(Stream, position / 1000d));
        }

        private void AfterLoad()
        {
            if (!StreamLoaded)
                throw new Exception("Kyaa! Cannot call AfterLoad if stream isn't loaded.");

            audioManager.Tracks.Add(this);

            Length = Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetLength(Stream)) * 1000;
            Frequency = Bass.ChannelGetInfo(Stream).Frequency;
        }

        private void CheckIfDisposed()
        {
            if (!StreamLoaded || IsDisposed)
                throw new Exception("Kyaa! Already disposed!");
        }
    }
}
