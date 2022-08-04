using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Water.Audio
{
    public interface IPlayable : IDisposable
    {
        public void Play();

        public void Pause();

        public void Stop();
    }
}
