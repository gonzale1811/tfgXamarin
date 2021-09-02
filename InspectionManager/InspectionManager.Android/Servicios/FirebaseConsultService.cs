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
            var inspectorNuevo = new Dictionary<string, Java.Lang.Object>
            {
                { "Apellidos", inspector.Apellidos },
                { "DNI", inspector.Dni },
                { "FechaNacimiento", inspector.FechaNacimiento.ToString() },
                { "Inspecciones", new ArrayList()},
                { "Nombre", inspector.Nombre },
                { "Password", inspector.Password },
                { "Username", inspector.Usuario }
            };
            document.Set(new HashMap(inspectorNuevo));
        }
    }
}