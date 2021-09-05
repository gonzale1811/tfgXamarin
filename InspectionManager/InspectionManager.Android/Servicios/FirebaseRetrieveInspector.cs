using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Util;
using InspectionManager.Droid.Extensiones;
using InspectionManager.Droid.Servicios;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseRetrieveInspector))]
namespace InspectionManager.Droid.Servicios
{
    public class FirebaseRetrieveInspector: IFirebaseRetrieve<Inspector>
    {
        private readonly string TAG = "MYAPP";

        public FirebaseRetrieveInspector()
        {
        }

        public IEnumerable<Inspector> GetAll()
        {
            return this.GetAllAsync().Result;
        }

        public Task<IEnumerable<Inspector>> GetAllAsync()
        {
            return DatabaseConnection.GetInstance.Collection("Inspectores").GetCollectionAsync<Inspector>();
        }

        public Inspector GetOne(string id)
        {
            var resultado = this.GetOneAsync(id).Result;
            Log.Info(TAG, resultado.Dni);
            return resultado;
        }

        public Task<Inspector> GetOneAsync(string id)
        {
            return DatabaseConnection.GetInstance.Collection("Inspectores").Document(id).GetDocumentAsync<Inspector>();
        }
    }
}
