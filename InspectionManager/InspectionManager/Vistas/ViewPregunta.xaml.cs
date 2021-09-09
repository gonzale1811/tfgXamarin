using System;
using System.Collections.Generic;
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
        private Plantilla plantillaEmpleada;
        private Bloque bloque;
        private Bloque bloqueInspeccion;
        private Picker picker;
        private Label puesto;
        private Button puestoSeleccionado;

        private readonly string MENSAJE = "Responde aqui a la pregunta";

        public ViewPregunta(Plantilla plantilla, Bloque bloqueSeleccionado)
        {
            InitializeComponent();

            plantillaEmpleada = plantilla;

            bloqueInspeccion = new Bloque(bloqueSeleccionado.Nombre);
            bloqueInspeccion.PreguntasTexto = bloqueSeleccionado.PreguntasTexto;
            bloqueInspeccion.PreguntasBoolean = bloqueSeleccionado.PreguntasBoolean;
            bloqueInspeccion.PreguntasValor = bloqueSeleccionado.PreguntasValor;

            camera = new CameraService();

            consult = DependencyService.Get<IFirebaseConsultService>();

            preguntasString = consult.GetPreguntasTextoByBloque(bloqueSeleccionado);
            preguntasBoolean = consult.GetPreguntasBooleanByBloque(bloqueSeleccionado);
            preguntasInt = consult.GetPreguntasValorByBloque(bloqueSeleccionado);

            puesto = new Label
            {
                Text = "Seleccione el puesto para el que rellena el bloque.",
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

            layoutPreguntas.Children.Add(picker);

            puestoSeleccionado = new Button
            {
                Text = "Seleccionar",
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
                bloqueInspeccion.PuestoTrabajo = picker.SelectedItem.ToString();
                layoutPreguntas.Children.Remove(puesto);
                layoutPreguntas.Children.Remove(picker);
                layoutPreguntas.Children.Remove(puestoSeleccionado);

                CargarPreguntasDelBloque();
            }
        }

        private void CargarPreguntasDelBloque()
        {
            Label tituloTexto = new Label();
            tituloTexto.Text = "Preguntas de texto";
            tituloTexto.HorizontalTextAlignment = TextAlignment.Center;
            tituloTexto.FontSize = 24;
            layoutPreguntas.Children.Add(tituloTexto);

            foreach (IPregunta<string> pregunta in preguntasString)
            {
                Label texto = new Label();
                texto.Text = pregunta.Nombre;
                Entry campoRespuesta = new Entry();
                campoRespuesta.Placeholder = MENSAJE;
                campoRespuesta.Keyboard = Keyboard.Text;
                layoutPreguntas.Children.Add(texto);
                layoutPreguntas.Children.Add(campoRespuesta);
            }

            Label tituloBoolean = new Label();
            tituloBoolean.Text = "Preguntas de verdadero/falso";
            tituloBoolean.HorizontalTextAlignment = TextAlignment.Center;
            tituloBoolean.FontSize = 24;
            Label explicacion = new Label();
            explicacion.Text = "Marque los check si es verdadero.";
            explicacion.FontSize = 16;
            layoutPreguntas.Children.Add(tituloBoolean);
            layoutPreguntas.Children.Add(explicacion);

            foreach (IPregunta<bool> pregunta1 in preguntasBoolean)
            {
                Label texto = new Label();
                texto.Text = pregunta1.Nombre;
                CheckBox respuesta = new CheckBox();
                respuesta.IsChecked = false;
                layoutPreguntas.Children.Add(texto);
                layoutPreguntas.Children.Add(respuesta);
            }

            Label tituloValor = new Label();
            tituloValor.Text = "Preguntas numericas";
            tituloValor.HorizontalTextAlignment = TextAlignment.Center;
            tituloValor.FontSize = 24;
            layoutPreguntas.Children.Add(tituloValor);

            foreach (IPregunta<int> pregunta2 in preguntasInt)
            {
                Label texto = new Label();
                texto.Text = pregunta2.Nombre;
                Entry campoRespuesta = new Entry();
                campoRespuesta.Keyboard = Keyboard.Numeric;
                layoutPreguntas.Children.Add(texto);
                layoutPreguntas.Children.Add(campoRespuesta);
            }

            StackLayout botones = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center
            };

            Button cancelar = new Button
            {
                Text = "Cancelar",
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            Button foto = new Button
            {
                ImageSource = "add_foto_black.png",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button aceptar = new Button
            {
                Text = "Aceptar",
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

        public void ProcesarAceptar(object sender, EventArgs e)
        {
            int numeroPreguntasTexto = preguntasString.Count;
            int numeroPreguntasBoolean = preguntasBoolean.Count;
            int numeroPreguntasInt = preguntasInt.Count;

            foreach(View v in layoutPreguntas.Children)
            {
                Type tipo = v.GetType();

                if (tipo.Equals(typeof(Entry)))
                {
                    var respuesta = (Entry)v;
                    if (numeroPreguntasTexto > 0)
                    {
                        preguntasString[numeroPreguntasTexto - 1].Responder(respuesta.Text);
                        numeroPreguntasTexto--;
                    }
                    else
                    {
                        preguntasInt[numeroPreguntasInt - 1].Responder(Int32.Parse(respuesta.Text));
                        numeroPreguntasBoolean--;
                    }
                }else if (tipo.Equals(typeof(CheckBox)))
                {
                    var respuesta = (CheckBox)v;
                    preguntasBoolean[numeroPreguntasBoolean - 1].Responder(respuesta.IsChecked);
                }
            }
        }

        public async void ProcesarFotografia(object sender, EventArgs e)
        {
            try
            {
                var resultado = await camera.TakePhoto();
                var subida = resultado;
                string url = consult.UploadFoto(subida);
                bloqueInspeccion.Fotografias.Add(url);
                await DisplayAlert("Correcto", "Fotografia realizada correctamente", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Error en la camara", "Ok");
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }
    }
}
