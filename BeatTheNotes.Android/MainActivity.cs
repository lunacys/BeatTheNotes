using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Jackhammer;

namespace Jackhammer.Android
{
    [Activity(Label = "Jackhammer.Android"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , ScreenOrientation = ScreenOrientation.UserLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new Jackhammer();
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }
    }
}

