using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Audio
{
    public interface IAudioService
    {
        public AudioManager Audio { get; set; }

        public bool Initialize();

        public void PlayTrack(string filePath);

        public void PlayEffect(string filePath, bool canOnlyPlayOnce, float rate = 1f, float pan = 0f);
    }
}
