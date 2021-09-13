using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Storage;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewPregunta : ContentPage
    {
        private IFirebaseConsultService consult;
        private CameraService camera;
        private List<IPregunta<string>> preguntasString;
        private List<IPregunta<bool>> preguntasBoolean;
        private List<IPregunta<int>> preguntasInt;
        private List<Bloque> bloquesInspeccion;
        private List<string> nombrePuestoBloque;
        private List<string> puestosSeleccionados;
        private Plantilla plantillaEmpleada;
        private Bloque bloque;
        private Inspeccion inspeccionProceso;
        private Bloque bloqueInspeccion;
        private Picker picker;
        private Label puesto;
        private Button puestoSeleccionado;

        private int contadorFotos;

        private readonly string MENSAJE = "Responde aqui a la pregunta";

        public ViewPregunta(Plantilla plantilla, Inspeccion inspeccionCreada, Bloque bloqueSeleccionado, List<Bloque> bloques)
        {
            InitializeComponent();

            contadorFotos = 1;

            inspeccionProceso = inspeccionCreada;
            bloquesInspeccion = bloques;
            nombrePuestoBloque = new List<string>();
            puestosSeleccionados = new List<string>();
            bloque = bloqueSeleccionado;

            foreach(Bloque bloqueLista in bloquesInspeccion)
            {
                puestosSeleccionados.Add(bloqueLista.PuestoTrabajo);
                nombrePuestoBloque.Add(bloqueLista.Nombre + "_" + bloqueLista.PuestoTrabajo);
            }

            plantillaEmpleada = plantilla;

            bloqueInspeccion = new Bloque(bloqueSeleccionado.Nombre);

            camera = new CameraService();

            consult = DependencyService.Get<IFirebaseConsultService>();

            preguntasString = consult.GetPreguntasTextoByBloque(bloqueSeleccionado);
            preguntasBoolean = consult.GetPreguntasBooleanByBloque(bloqueSeleccionado);
            preguntasInt = consult.GetPreguntasValorByBloque(bloqueSeleccionado);

            puesto = new Label
            {
                Text = "Seleccione el puesto para el que rellena el bloque.",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            puesto.FontSize = 24;

            layoutPreguntas.Children.Add(puesto);

            List<string> opcionesPicker = plantillaEmpleada.PuestosDelTipoTrabajo();

            picker = new Picker();

            picker.ItemsSource = opcionesPicker;
            picker.Title = "Seleccione el puesto de trabajo";
            picker.HorizontalTextAlignment = TextAlignment.Center;
            picker.TextColor = Color.White;

            layoutPreguntas.Children.Add(picker);

            puestoSeleccionado = new Button
            {
                Text = "Seleccionar",
                BackgroundColor = Color.PaleGoldenrod,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            puestoSeleccionado.Clicked += ProcesarPuestoSeleccionado;

            layoutPreguntas.Children.Add(puestoSeleccionado);
        }

        public async void ProcesarPuestoSeleccionado(object sender, EventArgs e)
        {
            if (picker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Para continuar seleccione un puesto de trabajo", "Ok");
            }
            else
            {
                if (puestosSeleccionados.Contains(picker.SelectedItem.ToString())&&(nombrePuestoBloque.Contains(bloque.Nombre+"_"+picker.SelectedItem.ToString())))
                {
                    await DisplayAlert("Error", "Ya ha rellenado un bloque para dicho puesto, seleccione otro", "Ok");
                }
                else
                {
                    bloqueInspeccion.PuestoTrabajo = picker.SelectedItem.ToString();
                    inspeccionProceso.Bloques.Add(bloqueInspeccion.IdBloque.ToString() + "_" + bloqueInspeccion.PuestoTrabajo);
                    layoutPreguntas.Children.Remove(puesto);
                    layoutPreguntas.Children.Remove(picker);
                    layoutPreguntas.Children.Remove(puestoSeleccionado);

                    CargarPreguntasDelBloque();
                }
            }
        }

        private void CargarPreguntasDelBloque()
        {
            if (preguntasString.Count > 0)
            {
                Label tituloTexto = new Label();
                tituloTexto.Text = "Preguntas de texto";
                tituloTexto.TextColor = Color.White;
                tituloTexto.HorizontalTextAlignment = TextAlignment.Center;
                tituloTexto.FontSize = 24;
                layoutPreguntas.Children.Add(tituloTexto);

                foreach (IPregunta<string> pregunta in preguntasString)
                {
                    Label texto = new Label();
                    texto.Text = pregunta.Nombre;
                    texto.TextColor = Color.White;
                    Entry campoRespuesta = new Entry();
                    campoRespuesta.Placeholder = MENSAJE;
                    campoRespuesta.PlaceholderColor = Color.White;
                    campoRespuesta.Keyboard = Keyboard.Text;
                    campoRespuesta.TextColor = Color.White;
                    layoutPreguntas.Children.Add(texto);
                    layoutPreguntas.Children.Add(campoRespuesta);
                }
            }

            if (preguntasBoolean.Count > 0)
            {
                Label tituloBoolean = new Label();
                tituloBoolean.Text = "Preguntas de verdadero/falso";
                tituloBoolean.HorizontalTextAlignment = TextAlignment.Center;
                tituloBoolean.FontSize = 24;
                tituloBoolean.TextColor = Color.White;
                Label explicacion = new Label();
                explicacion.Text = "Marque los check si es verdadero.";
                explicacion.FontSize = 16;
                explicacion.TextColor = Color.White;
                layoutPreguntas.Children.Add(tituloBoolean);
                layoutPreguntas.Children.Add(explicacion);

                foreach (IPregunta<bool> pregunta1 in preguntasBoolean)
                {
                    Label texto = new Label();
                    texto.Text = pregunta1.Nombre;
                    texto.TextColor = Color.White;
                    CheckBox respuesta = new CheckBox();
                    respuesta.IsChecked = false;
                    layoutPreguntas.Children.Add(texto);
                    layoutPreguntas.Children.Add(respuesta);
                }
            }

            if (preguntasInt.Count > 0)
            {
                Label tituloValor = new Label();
                tituloValor.Text = "Preguntas numericas";
                tituloValor.HorizontalTextAlignment = TextAlignment.Center;
                tituloValor.FontSize = 24;
                tituloValor.TextColor = Color.White;
                layoutPreguntas.Children.Add(tituloValor);

                foreach (IPregunta<int> pregunta2 in preguntasInt)
                {
                    Label texto = new Label();
                    texto.Text = pregunta2.Nombre;
                    texto.TextColor = Color.White;
                    Entry campoRespuesta = new Entry();
                    campoRespuesta.Placeholder = "Indique el valor correspondiente";
                    campoRespuesta.PlaceholderColor = Color.White;
                    campoRespuesta.Keyboard = Keyboard.Numeric;
                    campoRespuesta.TextColor = Color.White;
                    layoutPreguntas.Children.Add(texto);
                    layoutPreguntas.Children.Add(campoRespuesta);
                }
            }

            StackLayout botones = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            Button cancelar = new Button
            {
                Text = "Cancelar",
                BackgroundColor = Color.PaleGoldenrod,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Button foto = new Button
            {
                ImageSource = "add_foto_black.png",
                BackgroundColor = Color.PaleGoldenrod,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button aceptar = new Button
            {
                Text = "Aceptar",
                BackgroundColor = Color.PaleGoldenrod,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            cancelar.Clicked += ProcesarCancelar;
            foto.Clicked += ProcesarFotografia;
            aceptar.Clicked += ProcesarAceptar;

            botones.Children.Add(cancelar);
            botones.Children.Add(foto);
            botones.Children.Add(aceptar);

            layoutPreguntas.Children.Add(botones);
        }

        public async void ProcesarAceptar(object sender, EventArgs e)
        {
            int numeroPreguntasTexto = preguntasString.Count;
            int numeroPreguntasBoolean = preguntasBoolean.Count;
            int numeroPreguntasInt = preguntasInt.Count;

            List<IPregunta<string>> preguntasTextoRespondidas = new List<IPregunta<string>>();
            List<IPregunta<bool>> preguntasBooleanRespondidas = new List<IPregunta<bool>>();
            List<IPregunta<int>> preguntasIntRespondidas = new List<IPregunta<int>>();

            foreach(View v in layoutPreguntas.Children)
            {
                Type tipo = v.GetType();

                if (tipo.Equals(typeof(Entry)))
                {
                    var respuesta = (Entry)v;
                    if (numeroPreguntasTexto > 0)
                    {

                        IPregunta<string> preguntaTextoRespondida = new PreguntaTexto(preguntasString[numeroPreguntasTexto - 1].Nombre);
                        bloqueInspeccion.AddPreguntaTexto(preguntaTextoRespondida.IdPregunta.ToString());
                        preguntaTextoRespondida.Responder(respuesta.Text);
                        preguntasTextoRespondidas.Add(preguntaTextoRespondida);
                        numeroPreguntasTexto--;
                    }
                    else
                    {
                        IPregunta<int> preguntaIntRespondida = new PreguntaValor(preguntasInt[numeroPreguntasInt - 1].Nombre);
                        bloqueInspeccion.AddPreguntaValor(preguntaIntRespondida.IdPregunta.ToString());
                        var respuestaEntry = respuesta.Text;
                        if (respuestaEntry != null)
                        {
                            preguntaIntRespondida.Responder(Int32.Parse(respuesta.Text));
                        }
                        else
                        {
                            preguntaIntRespondida.Responder(0);
                        }
                        preguntasIntRespondidas.Add(preguntaIntRespondida);
                        numeroPreguntasBoolean--;
                    }
                }else if (tipo.Equals(typeof(CheckBox)))
                {
                    var respuesta = (CheckBox)v;
                    IPregunta<bool> preguntaBooleanRespondida = new PreguntaBoolean(preguntasBoolean[numeroPreguntasBoolean - 1].Nombre);
                    bloqueInspeccion.AddPreguntaBoolean(preguntaBooleanRespondida.IdPregunta.ToString());
                    preguntaBooleanRespondida.Responder(respuesta.IsChecked);
                    preguntasBooleanRespondidas.Add(preguntaBooleanRespondida);
                    numeroPreguntasBoolean--;
                }
            }

            bloquesInspeccion.Add(bloqueInspeccion);

            consult.AddBloqueInspeccion(bloqueInspeccion);
            consult.AddPreguntasTexto(preguntasTextoRespondidas, "PreguntasTextoInspeccion", true);
            consult.AddPreguntasBoolean(preguntasBooleanRespondidas, "PreguntasBooleanInspeccion", true);
            consult.AddPreguntasValor(preguntasIntRespondidas, "PreguntasValorInspeccion", true);

            await Navigation.PushModalAsync(new NavigationPage(new ViewBloques(inspeccionProceso, plantillaEmpleada, bloquesInspeccion)));
        }

        public async void ProcesarFotografia(object sender, EventArgs e)
        {
            try
            {
                var resultado = await camera.TakePhoto();
                var subida = resultado;

                var url = consult.UploadFoto(inspeccionProceso.IdInspeccion.ToString(), bloqueInspeccion.IdBloque.ToString() + "_" + bloqueInspeccion.PuestoTrabajo, contadorFotos, subida);

                bloqueInspeccion.Fotografias.Add(url);

                contadorFotos++;
                await DisplayAlert("Correcto", "Fotografia realizada correctamente", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Se ha producido un error al realizar la foto.", "Ok");
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            bool cancelar = await DisplayAlert("Precaución", "Si cancela perdera la información introducida, ¿desea continuar?", "Si", "No");
            if (cancelar)
            {
                consult.CancelarCreacionInspeccion(inspeccionProceso, bloquesInspeccion);
                await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
            }
        }
    }
}
