using System;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Threading.Tasks;

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
                { "FechaNacimiento", inspector.FechaNacimiento },
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

                        //DateTime fechaNacimiento = Convert.ToDateTime(fecha);

                        inspectorActual = new Inspector(item.Get("DNI").ToString(), item.Get("Nombre").ToString(), item.Get("Apellidos").ToString(), emailObtenido, item.Get("Password").ToString(), fecha);

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
                {"fechaInicio", inspeccion.FechaInicio },
                {"fechaFin", inspeccion.FechaFin },
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
                        resultado.Add(obtenido);
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

        public string UploadFoto(string idInspeccion, string idBloque, int foto, Stream imagen)
        {
            if(imagen != null)
            {
                var task = new FirebaseStorage("inspection-manager-609e2.appspot.com").Child(idInspeccion).Child(idBloque).Child("evidencia-"+foto+".png").PutAsync(imagen).TargetUrl;
                return task;
            }
            else
            {
                return null;
            }
        }

        public void AddBloqueInspeccion(Bloque bloque)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("BloquesInspeccion").Document(bloque.IdBloque.ToString()+"_"+bloque.PuestoTrabajo);

            int cont = 0;

            HashMap preguntasTexto = new HashMap();
            foreach(string preguntaTexto in bloque.PreguntasTexto)
            {
                preguntasTexto.Put(cont.ToString(), preguntaTexto);
                cont++;
            }

            cont = 0;

            HashMap preguntasBoolean = new HashMap();
            foreach(string preguntaBoolean in bloque.PreguntasBoolean)
            {
                preguntasBoolean.Put(cont.ToString(), preguntaBoolean);
                cont++;
            }

            cont = 0;

            HashMap preguntasValor = new HashMap();
            foreach(string preguntaValor in bloque.PreguntasValor)
            {
                preguntasValor.Put(cont.ToString(), preguntaValor);
                cont++;
            }

            cont = 0;

            HashMap fotografias = new HashMap();
            foreach(string foto in bloque.Fotografias)
            {
                fotografias.Put(cont.ToString(), foto);
                cont++;
            }

            var dictionary = new Dictionary<string, Java.Lang.Object>
            {
                {"idBloque", bloque.IdBloque.ToString()+"_"+bloque.PuestoTrabajo },
                {"nombre", bloque.Nombre },
                {"puestoDeTrabajo", bloque.PuestoTrabajo },
                {"fotografias", fotografias },
                {"preguntasTexto", preguntasTexto },
                {"preguntasBoolean", preguntasBoolean },
                {"preguntasValor", preguntasValor }
            };
            documentReference.Set(new HashMap(dictionary));
        }

        public void AddPreguntasTexto(List<IPregunta<string>> preguntasTexto, string coleccion, bool paraInspeccion)
        {
            foreach(IPregunta<string> preguntaTexto in preguntasTexto)
            {
                DocumentReference documentReference = DatabaseConnection.GetInstance.Collection(coleccion).Document(preguntaTexto.IdPregunta.ToString());
                HashMap respuesta = new HashMap();
                Dictionary<string, Java.Lang.Object> dictionary;

                if (paraInspeccion)
                {
                    respuesta.Put("respuesta", preguntaTexto.RespuestaPregunta.ValorRespuesta);
                    dictionary = new Dictionary<string, Java.Lang.Object>
                    {
                        {"idPregunta", preguntaTexto.IdPregunta.ToString() },
                        {"nombre", preguntaTexto.Nombre },
                        {"respuestaTexto", respuesta }
                    };
                }
                else
                {
                    dictionary = new Dictionary<string, Java.Lang.Object>
                    {
                        {"idPregunta", preguntaTexto.IdPregunta.ToString() },
                        {"nombre", preguntaTexto.Nombre }
                    };
                }

                documentReference.Set(new HashMap(dictionary));
            }
        }

        public void AddPreguntasBoolean(List<IPregunta<bool>> preguntasBoolean, string coleccion, bool paraInspeccion)
        {
            foreach(IPregunta<bool> preguntaBoolean in preguntasBoolean)
            {
                DocumentReference documentReference = DatabaseConnection.GetInstance.Collection(coleccion).Document(preguntaBoolean.IdPregunta.ToString());
                HashMap respuesta = new HashMap();
                Dictionary<string, Java.Lang.Object> dictionary;

                if (paraInspeccion)
                {
                    respuesta.Put("respuesta", preguntaBoolean.RespuestaPregunta.ValorRespuesta);
                    dictionary = new Dictionary<string, Java.Lang.Object>
                    {
                        {"idPregunta", preguntaBoolean.IdPregunta.ToString() },
                        {"nombre", preguntaBoolean.Nombre },
                        {"respuestaBoolean", respuesta }
                    };
                }
                else
                {
                    dictionary = new Dictionary<string, Java.Lang.Object>
                    {
                        {"idPregunta", preguntaBoolean.IdPregunta.ToString() },
                        {"nombre", preguntaBoolean.Nombre }
                    };
                }
                
                documentReference.Set(new HashMap(dictionary));
            }
        }

        public void AddPreguntasValor(List<IPregunta<int>> preguntasValor, string coleccion, bool paraInspeccion)
        {
            foreach(IPregunta<int> preguntaInt in preguntasValor)
            {
                DocumentReference documentReference = DatabaseConnection.GetInstance.Collection(coleccion).Document(preguntaInt.IdPregunta.ToString());
                HashMap respuesta = new HashMap();
                Dictionary<string, Java.Lang.Object> dictionary;

                if (paraInspeccion)
                {
                    respuesta.Put("respuesta", preguntaInt.RespuestaPregunta.ValorRespuesta);
                    dictionary = new Dictionary<string, Java.Lang.Object>
                    {
                        {"idPregunta", preguntaInt.IdPregunta.ToString() },
                        {"nombre", preguntaInt.Nombre },
                        {"respuestaValor", respuesta }
                    };
                }
                else
                {
                    dictionary = new Dictionary<string, Java.Lang.Object>
                    {
                        {"idPregunta", preguntaInt.IdPregunta.ToString() },
                        {"nombre", preguntaInt.Nombre }
                    };
                }

                documentReference.Set(new HashMap(dictionary));
            }
        }

        public void SetBloquesToInspeccion(Inspeccion inspeccion)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Inspecciones").Document(inspeccion.IdInspeccion.ToString());

            int cont = 0;

            HashMap bloques = new HashMap();
            foreach(string bloque in inspeccion.Bloques)
            {
                bloques.Put(cont.ToString(), bloque);
                cont++;
            }

            documentReference.Update("bloques", bloques);
        }

        public void CancelarCreacionInspeccion(Inspeccion inspeccion, List<Bloque> bloques)
        {
            FirebaseFirestore instancia = DatabaseConnection.GetInstance;
            FirebaseStorage instanciaStorage = new FirebaseStorage("inspection-manager-609e2.appspot.com");

            if (bloques != null)
            {
                if (bloques.Count > 0)
                {
                    foreach(Bloque b in bloques)
                    {
                        if (b.PreguntasBoolean.Count > 0)
                        {
                            foreach(string pB in b.PreguntasBoolean)
                            {
                                instancia.Collection("PreguntasBooleanInspeccion").Document(pB).Delete();
                            }
                        }

                        if(b.PreguntasTexto.Count > 0)
                        {
                            foreach(string pT in b.PreguntasTexto)
                            {
                                instancia.Collection("PreguntasTextoInspeccion").Document(pT).Delete();
                            }
                        }

                        if(b.PreguntasValor.Count > 0)
                        {
                            foreach(string pV in b.PreguntasValor)
                            {
                                instancia.Collection("PreguntasValorInspeccion").Document(pV).Delete();
                            }
                        }

                        if (b.Fotografias.Count > 0)
                        {
                            instanciaStorage.Child(inspeccion.IdInspeccion.ToString()).Child(b.IdBloque.ToString() + "_" + b.PuestoTrabajo).DeleteAsync();
                        }

                        instancia.Collection("BloquesInspeccion").Document(b.IdBloque.ToString() + "_" + b.PuestoTrabajo).Delete();
                    }
                }
            }

            if (inspeccion != null)
            {
                instancia.Collection("Inspecciones").Document(inspeccion.IdInspeccion.ToString()).Delete();
            }

            var carpeta = instanciaStorage.Child(inspeccion.IdInspeccion.ToString());

            if (carpeta != null)
            {
                carpeta.DeleteAsync();
            }
        }

        public void AddPlantilla(Plantilla plantilla)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Plantillas").Document(plantilla.IdPlantilla.ToString());
            HashMap bloques = new HashMap();
            HashMap versiones = new HashMap();
            var dictionary = new Dictionary<string, Java.Lang.Object>
            {
                {"idPlantilla", plantilla.IdPlantilla.ToString() },
                {"nombre", plantilla.Nombre },
                {"tipoTrabajo", plantilla.Trabajo.ToString() },
                {"versionActual", plantilla.VersionActual },
                {"versiones", versiones },
                {"bloques", bloques }
            };
            documentReference.Set(new HashMap(dictionary));
        }

        public void AddBloquePlantilla(Bloque bloque)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Bloques").Document(bloque.IdBloque.ToString());

            int cont = 0;

            HashMap preguntasTexto = new HashMap();
            foreach (string preguntaTexto in bloque.PreguntasTexto)
            {
                preguntasTexto.Put(cont.ToString(), preguntaTexto);
                cont++;
            }

            cont = 0;

            HashMap preguntasBoolean = new HashMap();
            foreach (string preguntaBoolean in bloque.PreguntasBoolean)
            {
                preguntasBoolean.Put(cont.ToString(), preguntaBoolean);
                cont++;
            }

            cont = 0;

            HashMap preguntasValor = new HashMap();
            foreach (string preguntaValor in bloque.PreguntasValor)
            {
                preguntasValor.Put(cont.ToString(), preguntaValor);
                cont++;
            }

            var dictionary = new Dictionary<string, Java.Lang.Object>
            {
                {"idBloque", bloque.IdBloque.ToString() },
                {"nombre", bloque.Nombre },
                {"preguntasTexto", preguntasTexto },
                {"preguntasBoolean", preguntasBoolean },
                {"preguntasValor", preguntasValor }
            };
            documentReference.Set(new HashMap(dictionary));
        }

        public void SetBloquesToPlantilla(Plantilla plantilla)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Plantillas").Document(plantilla.IdPlantilla.ToString());
            HashMap bloques = new HashMap();
            int cont = 0;

            foreach(string bloque in plantilla.BloquesPlantilla)
            {
                bloques.Put(cont.ToString(), bloque);
                cont++;
            }

            documentReference.Update("bloques", bloques);
        }

        public void CancelarCreacionPlantilla(Plantilla plantilla, List<Bloque> bloques)
        {
            FirebaseFirestore instancia = DatabaseConnection.GetInstance;
            
            if (bloques != null)
            {
                if (bloques.Count > 0)
                {
                    foreach (Bloque b in bloques)
                    {
                        if (b.PreguntasBoolean.Count > 0)
                        {
                            foreach (string pB in b.PreguntasBoolean)
                            {
                                instancia.Collection("PreguntasBoolean").Document(pB).Delete();
                            }
                        }

                        if (b.PreguntasTexto.Count > 0)
                        {
                            foreach (string pT in b.PreguntasTexto)
                            {
                                instancia.Collection("PreguntasTexto").Document(pT).Delete();
                            }
                        }

                        if (b.PreguntasValor.Count > 0)
                        {
                            foreach (string pV in b.PreguntasValor)
                            {
                                instancia.Collection("PreguntasValor").Document(pV).Delete();
                            }
                        }

                        instancia.Collection("Bloques").Document(b.IdBloque.ToString()).Delete();
                    }
                }
            }

            if (plantilla != null)
            {
                instancia.Collection("Plantillas").Document(plantilla.IdPlantilla.ToString()).Delete();
            }
        }

        public List<Inspeccion> GetInspeccionesByUsuario(Inspector inspector)
        {
            List<Inspeccion> resultado = new List<Inspeccion>();

            var task = DatabaseConnection.GetInstance.Collection("Inspecciones").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    var fechaI = document.Get("fechaInicio").ToString();

                    //DateTime fechaInicio = Convert.ToDateTime(fechaI);

                    var fechaF = document.Get("fechaFin").ToString();

                    //DateTime fechaFin = Convert.ToDateTime(fechaF);

                    Inspeccion obtenida = new Inspeccion(Guid.Parse(document.Get("idInspeccion").ToString()), document.Get("nombre").ToString(), fechaI, fechaF);

                    var direccion = document.Get("direccion") != null ? document.Get("direccion") : null;

                    if (direccion != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(direccion.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            if (value.Key == "calle")
                            {
                                obtenida.DireccionInspeccion.Calle = value.Value;
                            }else if (value.Key == "numero")
                            {
                                obtenida.DireccionInspeccion.Numero = value.Value;
                            }else if (value.Key == "localidad")
                            {
                                obtenida.DireccionInspeccion.Localidad = value.Value;
                            }else if (value.Key == "codigoPostal")
                            {
                                obtenida.DireccionInspeccion.CodigoPostal = value.Value;
                            }
                        }
                    }

                    var bloques = document.Get("bloques") != null ? document.Get("bloques") : null;

                    if (bloques != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(bloques.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string, string> value in dictionaryFromHashmap)
                        {
                            obtenida.Bloques.Add(value.Value);
                        }
                    }

                    if (inspector.Inspecciones.Contains(obtenida.IdInspeccion.ToString()))
                    {
                        resultado.Add(obtenida);
                    }
                }
            }

            return resultado;
        }

        public async void GenerarPdfInspeccion(Inspeccion inspeccion, List<Bloque> bloques)
        {
            var directory = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory, "InspectionManager").ToString();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fechaDeHoy = DateTime.Today.ToString();

            var path = Path.Combine(directory, inspeccion.Nombre + ".pdf");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var fs = new FileStream(path, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddTitle("Informe de Inspección");
            document.AddCreator("Inspection Manager");
            document.AddCreationDate();
            document.Open();

            iTextSharp.text.Font _fuenteTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 22, iTextSharp.text.Font.SYMBOL);

            iTextSharp.text.Font _fuenteSubTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 18, iTextSharp.text.Font.SYMBOL);

            iTextSharp.text.Font _fuenteSeccion = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14, iTextSharp.text.Font.SYMBOL);

            iTextSharp.text.Font _fuenteEstandar = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL);

            Paragraph titulo = new Paragraph("Informe de la inspeccion: " + inspeccion.Nombre, _fuenteTitulo);
            titulo.IndentationLeft = 50f;
            titulo.IndentationRight = 50f;
            titulo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

            document.Add(titulo);

            Table tabla = new Table(7);

            tabla.Width = 100;

            iTextSharp.text.Cell nombre = new iTextSharp.text.Cell(new Phrase("Nombre", _fuenteSeccion));
            nombre.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            nombre.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            iTextSharp.text.Cell fechaInicio = new iTextSharp.text.Cell(new Phrase("Fecha de inicio", _fuenteSeccion));
            fechaInicio.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            fechaInicio.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            iTextSharp.text.Cell fechaFin = new iTextSharp.text.Cell(new Phrase("Fecha de fin", _fuenteSeccion));
            fechaFin.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            fechaFin.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            iTextSharp.text.Cell calle = new iTextSharp.text.Cell(new Phrase("Calle", _fuenteSeccion));
            calle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            calle.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            iTextSharp.text.Cell numero = new iTextSharp.text.Cell(new Phrase("Número", _fuenteSeccion));
            numero.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            numero.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            iTextSharp.text.Cell localidad = new iTextSharp.text.Cell(new Phrase("Localidad", _fuenteSeccion));
            localidad.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            localidad.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            iTextSharp.text.Cell codigoPostal = new iTextSharp.text.Cell(new Phrase("Código postal", _fuenteSeccion));
            codigoPostal.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            codigoPostal.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            tabla.AddCell(nombre);
            tabla.AddCell(fechaInicio);
            tabla.AddCell(fechaFin);
            tabla.AddCell(calle);
            tabla.AddCell(numero);
            tabla.AddCell(localidad);
            tabla.AddCell(codigoPostal);

            nombre = new iTextSharp.text.Cell(new Phrase(inspeccion.Nombre, _fuenteEstandar));
            nombre.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            nombre.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            fechaInicio = new iTextSharp.text.Cell(new Phrase(inspeccion.FechaInicio, _fuenteEstandar));
            fechaInicio.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            fechaInicio.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            fechaFin = new iTextSharp.text.Cell(new Phrase(inspeccion.FechaFin, _fuenteEstandar));
            fechaFin.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            fechaFin.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            calle = new iTextSharp.text.Cell(new Phrase(inspeccion.DireccionInspeccion.Calle, _fuenteEstandar));
            calle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            calle.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            numero = new iTextSharp.text.Cell(new Phrase(inspeccion.DireccionInspeccion.Numero, _fuenteEstandar));
            numero.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            numero.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            localidad = new iTextSharp.text.Cell(new Phrase(inspeccion.DireccionInspeccion.Localidad, _fuenteEstandar));
            localidad.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            localidad.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            codigoPostal = new iTextSharp.text.Cell(new Phrase(inspeccion.DireccionInspeccion.CodigoPostal, _fuenteEstandar));
            codigoPostal.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            codigoPostal.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            tabla.AddCell(nombre);
            tabla.AddCell(fechaInicio);
            tabla.AddCell(fechaFin);
            tabla.AddCell(calle);
            tabla.AddCell(numero);
            tabla.AddCell(localidad);
            tabla.AddCell(codigoPostal);

            document.Add(tabla);

            Paragraph seccionBloques = new Paragraph("Bloques de la inspección", _fuenteSubTitulo);
            seccionBloques.IndentationLeft = 50f;
            seccionBloques.IndentationRight = 50f;
            seccionBloques.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

            document.Add(seccionBloques);

            foreach (Bloque bloque in bloques)
            {
                Paragraph tituloBloque = new Paragraph("Bloque: " + bloque.Nombre + "\nPuesto: " + bloque.PuestoTrabajo, _fuenteSubTitulo);
                tituloBloque.IndentationLeft = 50f;
                tituloBloque.IndentationRight = 50f;
                tituloBloque.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                document.Add(tituloBloque);

                List<IPregunta<string>> preguntasTexto = GetPreguntasTextoByBloqueInspeccion(bloque);

                if (preguntasTexto.Count > 0)
                {
                    Paragraph preguntasTextoTitle = new Paragraph("Preguntas de texto", _fuenteSeccion);
                    preguntasTextoTitle.IndentationLeft = 50f;
                    preguntasTextoTitle.IndentationRight = 50f;
                    preguntasTextoTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                    document.Add(preguntasTextoTitle);

                    foreach (IPregunta<string> preguntaTexto in preguntasTexto)
                    {
                        Paragraph respuestaTexto = new Paragraph("Pregunta: " + preguntaTexto.Nombre + "\nRespuesta: " + preguntaTexto.RespuestaPregunta.ValorRespuesta, _fuenteEstandar);
                        respuestaTexto.IndentationLeft = 50f;
                        respuestaTexto.IndentationRight = 50f;
                        respuestaTexto.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                        document.Add(respuestaTexto);
                    }
                }

                List<IPregunta<bool>> preguntasBoolean = GetPreguntasBooleanByBloqueInspeccion(bloque);

                if (preguntasBoolean.Count > 0)
                {
                    Paragraph preguntasBooleanTitle = new Paragraph("Preguntas de Verdadero/Falso", _fuenteSeccion);
                    preguntasBooleanTitle.IndentationLeft = 50f;
                    preguntasBooleanTitle.IndentationRight = 50f;
                    preguntasBooleanTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                    document.Add(preguntasBooleanTitle);

                    foreach (IPregunta<bool> preguntaBoolean in preguntasBoolean)
                    {
                        string respuestaTraducida = "";

                        if (preguntaBoolean.RespuestaPregunta.ValorRespuesta)
                        {
                            respuestaTraducida = "Verdadero";
                        }
                        else
                        {
                            respuestaTraducida = "Falso";
                        }

                        Paragraph respuestaBoolean = new Paragraph("Pregunta: " + preguntaBoolean.Nombre + "\nRespuesta: " + respuestaTraducida, _fuenteEstandar);
                        respuestaBoolean.IndentationLeft = 50f;
                        respuestaBoolean.IndentationRight = 50f;
                        respuestaBoolean.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                        document.Add(respuestaBoolean);
                    }
                }

                List<IPregunta<int>> preguntasValor = GetPreguntasValorByBloqueInspeccion(bloque);

                if (preguntasValor.Count > 0)
                {
                    Paragraph preguntasValorTitle = new Paragraph("Preguntas numéricas", _fuenteSeccion);
                    preguntasValorTitle.IndentationLeft = 50f;
                    preguntasValorTitle.IndentationRight = 50f;
                    preguntasValorTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                    document.Add(preguntasValorTitle);

                    foreach (IPregunta<int> preguntaValor in preguntasValor)
                    {
                        Paragraph respuestaValor = new Paragraph("Pregunta: " + preguntaValor.Nombre + "\nRespuesta: " + preguntaValor.RespuestaPregunta.ValorRespuesta, _fuenteEstandar);
                        respuestaValor.IndentationLeft = 50f;
                        respuestaValor.IndentationRight = 50f;
                        respuestaValor.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

                        document.Add(respuestaValor);
                    }
                }
            }

            Paragraph tituloImagenes = new Paragraph("Evidencias fotográficas", _fuenteSeccion);
            tituloImagenes.IndentationLeft = 50f;
            tituloImagenes.IndentationRight = 50f;
            tituloImagenes.Alignment = iTextSharp.text.Element.ALIGN_CENTER;

            document.Add(tituloImagenes);

            int cont = 1;

            foreach(Bloque bloque1 in bloques)
            {
                if (bloque1.Fotografias.Count > 0)
                {
                    foreach (string foto in bloque1.Fotografias)
                    {
                        var task = await new FirebaseStorage("inspection-manager-609e2.appspot.com").Child(inspeccion.IdInspeccion.ToString()).Child(bloque1.IdBloque.ToString() + "_" + bloque1.PuestoTrabajo).Child("evidencia-" + cont + ".png").GetDownloadUrlAsync();
                        iTextSharp.text.Image fotoActual = iTextSharp.text.Image.GetInstance(task);
                        fotoActual.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        fotoActual.ScalePercent(45f);

                        document.Add(fotoActual);

                        cont++;
                    }
                }

                cont = 1;
            }

            document.Close();
            writer.Close();
            fs.Close();
        }

        public List<Bloque> GetBloquesByInspeccion(Inspeccion inspeccion)
        {
            List<Bloque> resultado = new List<Bloque>();

            var task = DatabaseConnection.GetInstance.Collection("BloquesInspeccion").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    Bloque obtenido = new Bloque(document.Get("nombre").ToString(), document.Get("puestoDeTrabajo").ToString());
                    obtenido.IdBloque = Guid.Parse(document.Get("idBloque").ToString().Split("_")[0]);

                    var preguntasTexto = document.Get("preguntasTexto") != null ? document.Get("preguntasTexto") : null;

                    if (preguntasTexto != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(preguntasTexto.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenido.PreguntasTexto.Add(value.Value);
                        }
                    }

                    var preguntasBoolean = document.Get("preguntasBoolean") != null ? document.Get("preguntasBoolean") : null;

                    if (preguntasBoolean != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(preguntasBoolean.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenido.PreguntasBoolean.Add(value.Value);
                        }
                    }

                    var preguntasValor = document.Get("preguntasValor") != null ? document.Get("preguntasValor") : null;

                    if (preguntasValor != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(preguntasValor.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenido.PreguntasValor.Add(value.Value);
                        }
                    }

                    var fotografias = document.Get("fotografias") != null ? document.Get("fotografias") : null;

                    if (fotografias != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(fotografias.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenido.Fotografias.Add(value.Value);
                        }
                    }

                    if (inspeccion.Bloques.Contains(obtenido.IdBloque.ToString() + "_" + obtenido.PuestoTrabajo))
                    {
                        resultado.Add(obtenido);
                    }
                }
            }

            return resultado;
        }

        public List<IPregunta<string>> GetPreguntasTextoByBloqueInspeccion(Bloque bloque)
        {
            List<IPregunta<string>> resultado = new List<IPregunta<string>>();

            var task = DatabaseConnection.GetInstance.Collection("PreguntasTextoInspeccion").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    IPregunta<string> obtenida = new PreguntaTexto(Guid.Parse(document.Get("idPregunta").ToString()), document.Get("nombre").ToString());

                    var respuesta = document.Get("respuestaTexto") != null ? document.Get("respuestaTexto") : null;

                    if (respuesta != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(respuesta.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenida.Responder(value.Value);
                        }
                    }

                    if (bloque.PreguntasTexto.Contains(obtenida.IdPregunta.ToString()))
                    {
                        resultado.Add(obtenida);
                    }
                }
            }

            return resultado;
        }

        public List<IPregunta<bool>> GetPreguntasBooleanByBloqueInspeccion(Bloque bloque)
        {
            List<IPregunta<bool>> resultado = new List<IPregunta<bool>>();

            var task = DatabaseConnection.GetInstance.Collection("PreguntasBooleanInspeccion").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    IPregunta<bool> obtenida = new PreguntaBoolean(Guid.Parse(document.Get("idPregunta").ToString()), document.Get("nombre").ToString());

                    var respuesta = document.Get("respuestaBoolean") != null ? document.Get("respuestaBoolean") : null;

                    if (respuesta != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(respuesta.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenida.Responder(Boolean.Parse(value.Value));
                        }
                    }

                    if (bloque.PreguntasBoolean.Contains(obtenida.IdPregunta.ToString()))
                    {
                        resultado.Add(obtenida);
                    }
                }
            }

            return resultado;
        }

        public List<IPregunta<int>> GetPreguntasValorByBloqueInspeccion(Bloque bloque)
        {
            List<IPregunta<int>> resultado = new List<IPregunta<int>>();

            var task = DatabaseConnection.GetInstance.Collection("PreguntasValorInspeccion").Get();

            while (!task.IsSuccessful)
            {

            }

            var snapshot = (QuerySnapshot)task.Result;

            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;

                foreach(DocumentSnapshot document in documents)
                {
                    IPregunta<int> obtenida = new PreguntaValor(Guid.Parse(document.Get("idPregunta").ToString()), document.Get("nombre").ToString());

                    var respuesta = document.Get("respuestaValor") != null ? document.Get("respuestaValor") : null;

                    if (respuesta != null)
                    {
                        var dictionaryFromHashmap = new Android.Runtime.JavaDictionary<string, string>(respuesta.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> value in dictionaryFromHashmap)
                        {
                            obtenida.Responder(Convert.ToInt32(value.Value));
                        }
                    }

                    if (bloque.PreguntasValor.Contains(obtenida.IdPregunta.ToString()))
                    {
                        resultado.Add(obtenida);
                    }
                }
            }

            return resultado;
        }

        public void DeleteInspeccion(Inspector inspector, Inspeccion inspeccion, List<Bloque> bloques)
        {
            FirebaseFirestore instancia = DatabaseConnection.GetInstance;

            foreach(Bloque bloque in bloques)
            {
                foreach(string preguntaTexto in bloque.PreguntasTexto)
                {
                    instancia.Collection("PreguntasTextoInspeccion").Document(preguntaTexto).Delete();
                }

                foreach(string preguntaBoolean in bloque.PreguntasBoolean)
                {
                    instancia.Collection("PreguntasBooleanInspeccion").Document(preguntaBoolean).Delete();
                }

                foreach(string preguntaValor in bloque.PreguntasValor)
                {
                    instancia.Collection("PreguntasValorInspeccion").Document(preguntaValor).Delete();
                }

                instancia.Collection("BloquesInspeccion").Document(bloque.IdBloque.ToString() + "_" + bloque.PuestoTrabajo).Delete();
            }

            instancia.Collection("Inspecciones").Document(inspeccion.IdInspeccion.ToString()).Delete();

            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Inspectores").Document(inspector.Dni);

            HashMap inspecciones = new HashMap();
            int cont = 0;

            foreach(string i in inspector.Inspecciones)
            {
                inspecciones.Put(cont.ToString(), i);
            }

            documentReference.Update("Inspecciones", inspecciones);
        }

        public void ActualizarInspeccion(Inspeccion inspeccion)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Inspecciones").Document(inspeccion.IdInspeccion.ToString());

            HashMap direccion = new HashMap();
            direccion.Put("calle", inspeccion.DireccionInspeccion.Calle);
            direccion.Put("numero", inspeccion.DireccionInspeccion.Numero);
            direccion.Put("localidad", inspeccion.DireccionInspeccion.Localidad);
            direccion.Put("codigoPostal", inspeccion.DireccionInspeccion.CodigoPostal);

            documentReference.Update("nombre", inspeccion.Nombre);
            documentReference.Update("fechaInicio", inspeccion.FechaInicio.ToString());
            documentReference.Update("fechaFin", inspeccion.FechaFin.ToString());
            documentReference.Update("direccion", direccion);
        }

        public async Task<string> DonwloadImage(string idInspeccion, string idBloque, string nombreImagen)
        {
            var storeImage = await new FirebaseStorage("inspection-manager-609e2.appspot.com").Child(idInspeccion).Child(idBloque).Child(nombreImagen).GetDownloadUrlAsync();
            return storeImage;
        }

        public void ActualizarPreguntasTextoInspeccion(List<IPregunta<string>> preguntasTexto)
        {
            foreach(IPregunta<string> pregunta in preguntasTexto)
            {
                DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("PreguntasTextoInspeccion").Document(pregunta.IdPregunta.ToString());

                HashMap respuesta = new HashMap();
                respuesta.Put("respuesta", pregunta.RespuestaPregunta.ValorRespuesta);

                documentReference.Update("respuestaTexto", respuesta);
            }
        }

        public void ActualizarPreguntasBooleanInspeccion(List<IPregunta<bool>> preguntasBoolean)
        {
            foreach (IPregunta<bool> pregunta in preguntasBoolean)
            {
                DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("PreguntasBooleanInspeccion").Document(pregunta.IdPregunta.ToString());

                HashMap respuesta = new HashMap();
                respuesta.Put("respuesta", pregunta.RespuestaPregunta.ValorRespuesta);

                documentReference.Update("respuestaBoolean", respuesta);
            }
        }

        public void ActualizarPreguntasValorInspeccion(List<IPregunta<int>> preguntasValor)
        {
            foreach (IPregunta<int> pregunta in preguntasValor)
            {
                DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("PreguntasValorInspeccion").Document(pregunta.IdPregunta.ToString());

                HashMap respuesta = new HashMap();
                respuesta.Put("respuesta", pregunta.RespuestaPregunta.ValorRespuesta);

                documentReference.Update("respuestaValor", respuesta);
            }
        }

        public void ActualizarInformacionUsuario(Inspector inspector)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("Inspectores").Document(inspector.Dni);

            documentReference.Update("Nombre", inspector.Nombre);
            documentReference.Update("Apellidos", inspector.Apellidos);
            documentReference.Update("FechaNacimiento", inspector.FechaNacimiento.ToString());
        }

        public void UpdateFotografiasBloque(Bloque bloque)
        {
            DocumentReference documentReference = DatabaseConnection.GetInstance.Collection("BloquesInspeccion").Document(bloque.IdBloque.ToString() + "_" + bloque.PuestoTrabajo);

            HashMap fotografias = new HashMap();
            int cont = 0;
            foreach(string foto in bloque.Fotografias)
            {
                fotografias.Put(cont.ToString(), foto);
                cont++;
            }

            documentReference.Update("fotografias", fotografias);
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
    }
}