using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using InspectionManager.Droid.Servicios;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseConsultService))]
namespace InspectionManager.Droid.Servicios
{
    public class FirebaseConsultService : IFirebaseConsultService
    {

        public FirebaseConsultService()
        {
        }

        public void AddInspector(Inspector inspector)
        {
            DocumentReference document = DatabaseConnection.GetInstance.Collection("Inspectores").Document(inspector.Dni);
            Dictionary<string, object> inspectorNuevo = new Dictionary<string, object>
            {
                { "Apellidos", inspector.Apellidos },
                { "DNI", inspector.Dni },
                { "FechaNacimiento", inspector.FechaNacimiento },
                { "Inspecciones", inspector.Inspecciones },
                { "Nombre", inspector.Nombre },
                { "Password", inspector.Password },
                { "Username", inspector.Usuario }
            };
            document.Set(new HashMap(inspectorNuevo)).Wait();
        }
    }
}