using ManagedBass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Audio.BASS
{
    public class BASSAudioService : IAudioService
    {
        public AudioManager Audio { get; set; }

        public bool Initialize()
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
                Debug.WriteLine($"Kyaa >.<! Bass failed to initialize with error {error}.");
                return false;
            }
            else return true;
        }

        public void PlayTrack(string filePath) => new BASSAudioTrack(Audio, filePath).Play();
    }
}
