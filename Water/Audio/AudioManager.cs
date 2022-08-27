using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Water.Audio.BASS;

namespace Water.Audio
{
    public class AudioManager
    {
        public List<IAudioTrack> Tracks { get; private set; } = new();

        public List<IEffectTrack> Effects { get; private set; } = new();

        public double MasterVolume { get; set; } = 1;
        public double MusicVolume { get; set; } = 0.5;
        public double EffectVolume { get; set; } = 1;

        private readonly IAudioService audioService;

        public AudioManager()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                audioService = new BASSAudioService();
        }

        /// <summary>
        /// Initializes the audio service
        /// </summary>
        /// <returns>False if audio playback is not available</returns>
        public bool Initialize()
        {
            audioService.Audio = this;
            return audioService?.Initialize() ?? false;
        }

        public void PlayTrack(string filePath) => audioService?.PlayTrack(filePath);

        public void SwitchToTrack(string filePath)
        {
            StopPlayingAllTracks();
            PlayTrack(filePath);
        }

        public void StopPlayingAllTracks()
        {
            foreach (var track in Tracks)
            {
                track.Stop();
            }
        }

        public void PlayEffect(string filePath, bool canOnlyPlayOnce, float rate = 0f, float pan = 0f) => audioService?.PlayEffect(filePath, canOnlyPlayOnce, rate, pan);

        private List<IAudioTrack> tracksToRemove = new();
        private List<IEffectTrack> effectsToRemove = new();

        public void Update()
        {
            foreach (var track in Tracks)
            {
                if (track is IAudioTrack t)
                {
                    if (t.IsLeftOver && t.AutoDispose)
                        t.Dispose();

                    if (t.IsDisposed)
                    {
                        tracksToRemove.Add(t);
                        continue;
                    }
                }
            }

            foreach (var effect in Effects)
            {
                if (effect.IsLeftOver)
                    effect.Dispose();
                effectsToRemove.Add(effect);
            }

            foreach (var track in tracksToRemove)
                Tracks.Remove(track);
            tracksToRemove.Clear();

            foreach (var effect in effectsToRemove)
                Effects.Remove(effect);
            effectsToRemove.Clear();
        }
    }
}
