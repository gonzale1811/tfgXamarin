using System;
using System.Collections.Generic;
using Android.Gms.Tasks;
using Firebase.Firestore;
using InspectionManager.Droid.Servicios;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Java.Util;
using Xamarin.Forms;
using Android.Util;

[assembly: Dependency(typeof(FirebaseConsultService))]
namespace InspectionManager.Droid.Servicios
{
    public class FirebaseConsultService : Java.Lang.Object, IFirebaseConsultService, IOnSuccessListener
    {

        private List<Inspector> inspectores;
        private readonly string TAG = "MYAPP";

        public FirebaseConsultService()
        {
            inspectores = new List<Inspector>();
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

        public List<Inspector> GetInspectores()
        {
            DatabaseConnection.GetInstance.Collection("Inspectores").Get().AddOnSuccessListener(this);
            return inspectores;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                inspectores.Clear();

                foreach(DocumentSnapshot item in documents)
                {
                    var fecha = item.Get("FechaNacimiento").ToString();
                    Log.Info(TAG, "Se ha encontrado la fecha: " + fecha);

                    string[] valores = fecha.Split("/");

                    string[] otrosValores = valores[2].Split(" ");

                    DateTime fechaNacimiento = new DateTime(Convert.ToInt32(otrosValores[0]), Convert.ToInt32(valores[1]), Convert.ToInt32(valores[0]));

                    Log.Info(TAG, "Fecha tras haberla convertido: " + fechaNacimiento);

                    Inspector inspector = new Inspector(item.Get("DNI").ToString(), item.Get("Nombre").ToString(), item.Get("Apellidos").ToString(), item.Get("Username").ToString(), item.Get("Password").ToString(), fechaNacimiento);

                    inspectores.Add(inspector);
                }
            }
            else
            {
                Log.Info(TAG, "No se han encontrado datos");
            }
        }
    }
}