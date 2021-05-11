using System;
using System.Collections.Generic;
using System.Text;

namespace Sirene.Services
{
    public interface IPlaySoundService
    {
        void PlaySystemSound();
        void StopSystemSound();
    }
}
