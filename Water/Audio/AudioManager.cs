using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;
using System.Diagnostics;

namespace Water.Audio
{
    public class AudioManager
    {
        public List<IPlayable> Tracks { get; private set; } = new();

        public double MasterVolume { get; set; } = 1;
        public double MusicVolume { get; set; } = 0.5;
        public double EffectVolume { get; set; } = 1;

        public AudioManager()
        {

        }

        public void Initialize()
        {
            var n = 0;
            var info = new DeviceInfo();
            while (Bass.GetDeviceInfo(n, out info))
            {
                Debug.WriteLine($"{n}: Default?: {info.IsDefault} Name: {info.Name}");
                n++;
            }

            if (!Bass.Init(-1))
            {
                var error = Bass.LastError;
                throw new Exception($"Kyaa! Bass failed to initialize with error {error}.");
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var track in Tracks)
            {
                if (track is AudioTrack t)
                {
                    if (t.IsLeftOver && t.AutoDispose)
                        t.Dispose();

                    if (t.IsDisposed)
                    {
                        Tracks.Remove(t);
                        continue;
                    }
                }
            }
        }
    }
}
