using System;
using System.Collections.Generic;
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
    public class FirebaseConsultService : Java.Lang.Object, IFirebaseConsultService
    {

        private readonly string TAG = "MYAPP";
        private string emailActual = "";

        public FirebaseConsultService()
        {
        }

        public void AddInspector(Inspector inspector)
        {
            DocumentReference document = DatabaseConnection.GetInstance.Collection("Inspectores").Document(inspector.Dni);

            HashMap mapaInspecciones = new HashMap();

            var inspectorNuevo = new Dictionary<string, Java.Lang.Object>
            {
                { "Apellidos", inspector.Apellidos },
                { "DNI", inspector.Dni },
                { "FechaNacimiento", inspector.FechaNacimiento.ToString() },
                { "Inspecciones", mapaInspecciones},
                { "Nombre", inspector.Nombre },
                { "Password", inspector.Password },
                { "Username", inspector.Usuario }
            };
            document.Set(new HashMap(inspectorNuevo));
        }

        public Inspector GetInspectorByEmail(string email)
        {
            Inspector inspectorActual = null;
            emailActual = email;
            var tarea = DatabaseConnection.GetInstance.Collection("Inspectores").Get();//.AddOnSuccessListener(this);
            while (!tarea.IsSuccessful)
            {

            }
            var snapshot = (QuerySnapshot)tarea.Result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach (DocumentSnapshot item in documents)
                {
                    var emailObtenido = item.Get("Username").ToString();

                    if (emailObtenido == emailActual)
                    {
                        var fecha = item.Get("FechaNacimiento").ToString();

                        string[] valores = fecha.Split("/");

                        string[] otrosValores = valores[2].Split(" ");

                        DateTime fechaNacimiento = new DateTime(Convert.ToInt32(otrosValores[0]), Convert.ToInt32(valores[1]), Convert.ToInt32(valores[0]));

                        inspectorActual = new Inspector(item.Get("DNI").ToString(), item.Get("Nombre").ToString(), item.Get("Apellidos").ToString(), emailObtenido, item.Get("Password").ToString(), fechaNacimiento);

                        List<string> inspeccionesObtenidas = new List<string>();

                        var inspecciones = item.Get("Inspecciones") != null ? item.Get("Inspecciones") : null;

                        if (inspecciones != null)
                        {
                            var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(inspecciones.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                            foreach (KeyValuePair<string, string> value in dictionaryFromHashmap)
                            {
                                inspeccionesObtenidas.Add(value.Value);
                            }
                        }
                        inspectorActual.Inspecciones = inspeccionesObtenidas;
                    }
                }
            }
            else
            {
                Log.Info(TAG, "No se han encontrado datos");
                return inspectorActual;
            }

            return inspectorActual;
        }
    }
}