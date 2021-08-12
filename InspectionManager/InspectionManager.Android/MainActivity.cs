using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;

namespace InspectionManager.Droid
{
    [Activity(Label = "InspectionManager", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static FirebaseApp app;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            InitFirebaseAuth();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder().
                SetApplicationId("inspection-manager-609e2").
                SetApiKey("AIzaSyCU0_ZBKeSNvdZMvpWnsBQW8zXQqZEEDD4")
                .SetDatabaseUrl("https://inspection-manager-609e2-default-rtdb.europe-west1.firebasedatabase.app/").Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options, "FirebaseSample");
        }
    }
}