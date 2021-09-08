using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using InspectionManager.Droid.Servicios;
using Plugin.CurrentActivity;

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
            DatabaseConnection.Init(this);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder().
                SetProjectId("inspection-manager-609e2").
                SetApplicationId("1:128769436570:android:f77e04f7260560a5d9e823").
                SetApiKey("AIzaSyCU0_ZBKeSNvdZMvpWnsBQW8zXQqZEEDD4").
                SetDatabaseUrl("https://inspection-manager-609e2-default-rtdb.europe-west1.firebasedatabase.app").
                SetStorageBucket("inspection-manager-609e2.appspot.com").Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options, "InspectorManager");
        }
    }
}