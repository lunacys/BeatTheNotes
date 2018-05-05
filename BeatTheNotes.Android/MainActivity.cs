using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using BeatTheNotes.Shared;

namespace BeatTheNotes.Android
{
    [Activity(Label = "BeatTheNotes.Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.FullUser
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new GameRoot();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

