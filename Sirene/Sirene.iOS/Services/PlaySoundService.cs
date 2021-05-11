using AudioToolbox;
using AVFoundation;
using Foundation;
using Sirene.iOS.Services;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PlaySoundService))]
namespace Sirene.iOS.Services
{
    public class PlaySoundService : IPlaySoundService
    {
        SystemSound sound;
        public void PlaySystemSound()
        {
            AVAudioSession audioSession = AVAudioSession.SharedInstance();
            NSError error;
            audioSession.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out error);

            sound = new SystemSound(1151); //1304 (Alarm)
            sound.PlaySystemSound();
        }

        public void StopSystemSound()
        {
            sound.Close();
        }
    }
}