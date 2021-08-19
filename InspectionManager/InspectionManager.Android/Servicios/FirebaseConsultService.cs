using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using InspectionManager.Droid.Servicios;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseConsultService))]
namespace InspectionManager.Droid.Servicios
{
    public class FirebaseConsultService: IFirebaseConsultService
    {
        private FirebaseFirestore database;

        public FirebaseConsultService(Android.Content.Context context)
        {
            DatabaseConnection.Init(context);
            database = DatabaseConnection.GetInstance;
        }

        public Task<bool> AddInspector(Inspector inspector)
        {
            throw new NotImplementedException();
        }
    }
}
