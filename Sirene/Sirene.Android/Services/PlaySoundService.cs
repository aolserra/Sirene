using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sirene.Droid.Services;
using Sirene.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(PlaySoundService))]
namespace Sirene.Droid.Services
{
    public class PlaySoundService : IPlaySoundService
    {
        Ringtone rt;
        public void PlaySystemSound()
        {
            //If the ringer mode is different than "Normal" (2), enables the normal mode.
            AudioManager am = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
            if (!am.RingerMode.Equals(2))
            {
                am.RingerMode = RingerMode.Normal;
                //am.SetVibrateSetting(VibrateType.Ringer, VibrateSetting.On);
            }

            Android.Net.Uri uri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
            rt = RingtoneManager.GetRingtone(MainActivity.instance.ApplicationContext, uri);
            rt.Play();

        }

        public void StopSystemSound()
        {
            rt.Stop();
        }
    }
}