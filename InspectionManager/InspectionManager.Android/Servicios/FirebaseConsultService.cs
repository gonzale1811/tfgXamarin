﻿using System;
using System.Collections.Generic;
using Firebase.Firestore;
using InspectionManager.Droid.Servicios;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Java.Util;
using Xamarin.Forms;
using Android.Util;
using Android.Gms.Tasks;
using Firebase.Storage;
using System.IO;

[assembly: Dependency(typeof(FirebaseConsultService))]
namespace InspectionManager.Droid.Servicios
{
    public class FirebaseConsultService : Java.Lang.Object, IFirebaseConsultService, IOnSuccessListener, IOnFailureListener
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
            var tarea = DatabaseConnection.GetInstance.Collection("Inspectores").Get();
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

        public void AddInspeccion(Inspeccion inspeccion)
        {
            DocumentReference document = DatabaseConnection.GetInstance.Collection("Inspecciones").Document(inspeccion.IdInspeccion.ToString());
            HashMap direccion = new HashMap();
            direccion.Put("calle", inspeccion.DireccionInspeccion.Calle);
            direccion.Put("numero", inspeccion.DireccionInspeccion.Numero);
            direccion.Put("localidad", inspeccion.DireccionInspeccion.Localidad);
            direccion.Put("codigoPostal", inspeccion.DireccionInspeccion.CodigoPostal);
            HashMap bloques = new HashMap();
            var inspeccionNueva = new Dictionary<string, Java.Lang.Object>
            {
                {"idInspeccion",inspeccion.IdInspeccion.ToString() },
                {"nombre", inspeccion.Nombre },
                {"fechaInicio", inspeccion.FechaInicio.ToString() },
                {"fechaFin", inspeccion.FechaFin.ToString() },
                {"direccion", direccion },
                {"bloques", bloques }
            };
            document.Set(new HashMap(inspeccionNueva));
        }

        public void AddInspeccionToInspector(Inspector inspector, Inspeccion inspeccion)
        {
            DocumentReference document = DatabaseConnection.GetInstance.Collection("Inspectores").Document(inspector.Dni);
            int cont = 0;
            HashMap listaResultado = new HashMap();
            foreach(string posicion in inspector.Inspecciones)
            {
                if (posicion != null)
                {
                    listaResultado.Put(cont.ToString(), posicion);
                    cont++;
                }
            }
            document.Update("Inspecciones", listaResultado);
        }

        public List<Plantilla> GetPlantillas()
        {
            List<Plantilla> resultado = new List<Plantilla>();
            var task = DatabaseConnection.GetInstance.Collection("Plantillas").Get();
            while (!task.IsSuccessful)
            {

            }
            var snapshot = (QuerySnapshot)task.Result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    TipoTrabajo tipo = GetTipoTrabajoByString(document.Get("tipoTrabajo").ToString());
                    Plantilla nueva = new Plantilla(document.Get("nombre").ToString(), tipo, document.Get("versionActual").ToString());
                    nueva.IdPlantilla = Guid.Parse(document.Get("idPlantilla").ToString());

                    List<string> versionesObtenidas = new List<string>();
                    List<string> bloquesObtenidos = new List<string>();

                    var versiones = document.Get("versiones") != null ? document.Get("versiones") : null;
                    var bloques = document.Get("bloques") != null ? document.Get("bloques") : null;

                    if (versiones != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(versiones.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach (KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            versionesObtenidas.Add(value.Value);
                        }
                    }

                    if (bloques != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(bloques.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach (KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            bloquesObtenidos.Add(value.Value);
                        }
                    }

                    nueva.Versiones = versionesObtenidas;
                    nueva.BloquesPlantilla = bloquesObtenidos;

                    resultado.Add(nueva);
                }
            }

            return resultado;
        }

        public List<Bloque> GetBloquesByPlantilla(Plantilla p)
        {
            List<Bloque> resultado = new List<Bloque>();
            var task = DatabaseConnection.GetInstance.Collection("Bloques").Get();
            while (!task.IsSuccessful)
            {

            }
            var snapshot = (QuerySnapshot)task.Result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    Bloque obtenido = new Bloque(document.Get("nombre").ToString());
                    obtenido.IdBloque = Guid.Parse(document.Get("idBloque").ToString());

                    List<string> preguntasTextoObtenidas = new List<string>();
                    List<string> preguntasBooleanObtenidas = new List<string>();
                    List<string> preguntasValorObtenidas = new List<string>();

                    var preguntasTexto = document.Get("preguntasTexto") != null ? document.Get("preguntasTexto") : null;
                    var preguntasBoolean = document.Get("preguntasBoolean") != null ? document.Get("preguntasBoolean") : null;
                    var preguntasValor = document.Get("preguntasValor") != null ? document.Get("preguntasValor") : null;

                    if (preguntasTexto != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(preguntasTexto.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach (KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            preguntasTextoObtenidas.Add(value.Value);
                        }
                    }

                    if (preguntasBoolean != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(preguntasBoolean.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach (KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            preguntasBooleanObtenidas.Add(value.Value);
                        }
                    }

                    if (preguntasTexto != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(preguntasValor.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach (KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            preguntasValorObtenidas.Add(value.Value);
                        }
                    }

                    obtenido.PreguntasTexto = preguntasTextoObtenidas;
                    obtenido.PreguntasBoolean = preguntasBooleanObtenidas;
                    obtenido.PreguntasValor = preguntasValorObtenidas;

                    if (p.BloquesPlantilla.Contains(obtenido.IdBloque.ToString()))
                    {
                        Log.Info(TAG, "ESTE ES DE LA PLANTILLA");
                        resultado.Add(obtenido);
                    }
                    else
                    {
                        Log.Info(TAG, "ESTE NO ES DE LA PLANTILLA");
                    }
                }
            }

            return resultado;
        }

        public List<IPregunta<string>> GetPreguntasTextoByBloque(Bloque bloque)
        {
            List<IPregunta<string>> resultado = new List<IPregunta<string>>();
            var task = DatabaseConnection.GetInstance.Collection("PreguntasTexto").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    IPregunta<string> nueva = new PreguntaTexto(document.Get("nombre").ToString());
                    nueva.IdPregunta = Guid.Parse(document.Get("idPregunta").ToString());

                    if (bloque.PreguntasTexto.Contains(nueva.IdPregunta.ToString()))
                    {
                        resultado.Add(nueva);
                    }
                }
            }

            return resultado;
        }

        public List<IPregunta<bool>> GetPreguntasBooleanByBloque(Bloque bloque)
        {
            List<IPregunta<bool>> resultado = new List<IPregunta<bool>>();
            var task = DatabaseConnection.GetInstance.Collection("PreguntasBoolean").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    IPregunta<bool> nueva = new PreguntaBoolean(Guid.Parse(document.Get("idPregunta").ToString()), document.Get("nombre").ToString());

                    if (bloque.PreguntasBoolean.Contains(nueva.IdPregunta.ToString()))
                    {
                        resultado.Add(nueva);
                    }
                }
            }

            return resultado;
        }

        public List<IPregunta<int>> GetPreguntasValorByBloque(Bloque bloque)
        {
            List<IPregunta<int>> resultado = new List<IPregunta<int>>();
            var task = DatabaseConnection.GetInstance.Collection("PreguntasValor").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    IPregunta<int> nueva = new PreguntaValor(Guid.Parse(document.Get("idPregunta").ToString()), document.Get("nombre").ToString());

                    if (bloque.PreguntasValor.Contains(nueva.IdPregunta.ToString()))
                    {
                        resultado.Add(nueva);
                    }
                }
            }

            return resultado;
        }

        public string UploadFoto(Stream imagen)
        {
            if(imagen != null)
            {
                var task = new FirebaseStorage("inspection-manager-609e2.appspot.com").Child("prueba").Child("image2.jpg").PutAsync(imagen).TargetUrl;
                return task;
            }
            else
            {
                return null;
            }
        }

        private TipoTrabajo GetTipoTrabajoByString(string tipo)
        {
            switch (tipo)
            {
                case "Obra":
                    return TipoTrabajo.Obra;
                case "Oficina":
                    return TipoTrabajo.Oficina;
                case "Fabrica":
                    return TipoTrabajo.Fabrica;
                case "Servicios":
                    return TipoTrabajo.Servicios;
                default:
                    return TipoTrabajo.Oficina;
            }
        }

        void IOnSuccessListener.OnSuccess(Java.Lang.Object result)
        {
            Log.Info(TAG, "SE HA SUBIDO LA IMAGEN CORRECTAMENTE");
        }

        void IOnFailureListener.OnFailure(Java.Lang.Exception e)
        {
            Log.Info(TAG, "NO SE HA PODIDO SUBIR LA IMAGEN, CAUSA = "+e.Message);
        }
    }
}