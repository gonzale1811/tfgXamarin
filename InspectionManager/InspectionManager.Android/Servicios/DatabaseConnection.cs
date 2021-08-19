using System;
using Firebase;
using Firebase.Firestore;

namespace InspectionManager.Droid.Servicios
{
    public static class DatabaseConnection
    {
        public static FirebaseApp app;
        public static FirebaseFirestore GetInstance
        {
            get
            {
                return FirebaseFirestore.GetInstance(app);
            }
        }

        public static string AppName { get; } = "InspectionManager";

        public static void Init(Android.Content.Context context)
        {
            var options = new FirebaseOptions.Builder().
                SetProjectId("inspection-manager-609e2").
                SetApplicationId("1:128769436570:android:f77e04f7260560a5d9e823").
                SetApiKey("AIzaSyCU0_ZBKeSNvdZMvpWnsBQW8zXQqZEEDD4").
                SetDatabaseUrl("https://inspection-manager-609e2-default-rtdb.europe-west1.firebasedatabase.app").
                SetStorageBucket("inspection-manager-609e2.appspot.com").Build();
            app = FirebaseApp.InitializeApp(context, options, "InspectorManager");
        }
    }
}
