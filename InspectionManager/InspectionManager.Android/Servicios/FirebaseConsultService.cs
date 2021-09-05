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

        private Inspector inspectorActual;
        private readonly string TAG = "MYAPP";
        private string emailActual = "";

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

        public Inspector GetInspectorByEmail(string email)
        {
            emailActual = email;
            Task tarea = DatabaseConnection.GetInstance.Collection("Inspectores").Get().AddOnSuccessListener(this);

            return inspectorActual;
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot item in documents)
                {
                    var emailObtenido = item.Get("Username").ToString();

                    if (emailObtenido == emailActual)
                    {
                        var fecha = item.Get("FechaNacimiento").ToString();

                        string[] valores = fecha.Split("/");

                        string[] otrosValores = valores[2].Split(" ");

                        DateTime fechaNacimiento = new DateTime(Convert.ToInt32(otrosValores[0]), Convert.ToInt32(valores[1]), Convert.ToInt32(valores[0]));

                        inspectorActual = new Inspector(item.Get("DNI").ToString(), item.Get("Nombre").ToString(), item.Get("Apellidos").ToString(), emailObtenido, item.Get("Password").ToString(), fechaNacimiento);

                        Log.Info(TAG, "DNI: " + inspectorActual.Dni);
                        Log.Info(TAG, "Nombre: " + inspectorActual.Nombre);
                        Log.Info(TAG, "Apellidos: " + inspectorActual.Apellidos);
                        Log.Info(TAG, "Correo: " + inspectorActual.Usuario);
                        Log.Info(TAG, "Password: " + inspectorActual.Password);
                        Log.Info(TAG, "Fecha nacimiento: " + inspectorActual.FechaNacimiento);
                    }
                }
            }
            else
            {
                Log.Info(TAG, "No se han encontrado datos");
            }
        }
    }
}