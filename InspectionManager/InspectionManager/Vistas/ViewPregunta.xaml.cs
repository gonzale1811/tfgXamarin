using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Work;
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
        private Bloque bloque;
        private Image imagen;

        private readonly string MENSAJE = "Responde aqui a la pregunta";

        public ViewPregunta(Bloque bloqueSeleccionado)
        {
            InitializeComponent();

            camera = new CameraService();

            consult = DependencyService.Get<IFirebaseConsultService>();

            preguntasString = consult.GetPreguntasTextoByBloque(bloqueSeleccionado);
            preguntasBoolean = consult.GetPreguntasBooleanByBloque(bloqueSeleccionado);
            preguntasInt = consult.GetPreguntasValorByBloque(bloqueSeleccionado);

            Label tituloTexto = new Label();
            tituloTexto.Text = "Preguntas de texto";
            tituloTexto.FontSize = 24;
            layoutPreguntas.Children.Add(tituloTexto);

            foreach(IPregunta<string> pregunta in preguntasString)
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
            tituloBoolean.FontSize = 24;
            Label explicacion = new Label();
            explicacion.Text = "Marque los check si es verdadero.";
            explicacion.FontSize = 16;
            layoutPreguntas.Children.Add(tituloBoolean);
            layoutPreguntas.Children.Add(explicacion);

            foreach(IPregunta<bool> pregunta1 in preguntasBoolean)
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
            tituloValor.FontSize = 24;
            layoutPreguntas.Children.Add(tituloValor);

            foreach(IPregunta<int> pregunta2 in preguntasInt)
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
                Text = "Foto",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button aceptar = new Button
            {
                Text = "Aceptar",
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            Button descargar = new Button
            {
                Text = "Descargar",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            cancelar.Clicked += ProcesarCancelar;
            foto.Clicked += ProcesarFotografia;
            aceptar.Clicked += ProcesarAceptar;
            descargar.Clicked += ProcesarDescargar;

            imagen = new Image();

            botones.Children.Add(cancelar);
            botones.Children.Add(foto);
            botones.Children.Add(aceptar);
            botones.Children.Add(descargar);

            layoutPreguntas.Children.Add(botones);
            layoutPreguntas.Children.Add(imagen);
        }

        private void ProcesarDescargar(object sender, EventArgs e)
        {
            imagen.Source = Xamarin.Forms.ImageSource.FromUri(new Uri("https://firebasestorage.googleapis.com/v0/b/inspection-manager-609e2.appspot.com/o/prueba%2Fcomo_extraer_un_texto_de_una_imagen.jpg?alt=media&token=69167d0b-dc25-462e-bd8f-1993ad124c65"));
        }

        public void ProcesarAceptar(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public async void ProcesarFotografia(object sender, EventArgs e)
        {
            try
            {
                var resultado = await camera.TakePhoto();
                var subida = resultado;
                consult.UploadFoto(subida);
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

        private byte[] ImageSourceToByteArray(Xamarin.Forms.ImageSource imagen)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imagen;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            byte[] bytesAvaliable = new byte[stream.Length];
            stream.Read(bytesAvaliable, 0, bytesAvaliable.Length);
            return bytesAvaliable;
        }
    }
}
