using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewPreguntasInspeccion : ContentPage
    {
        private IFirebaseConsultService consult;
        private CameraService camera;

        private Inspeccion inspeccion;
        private Bloque bloque;
        private List<IPregunta<string>> preguntasString;
        private List<IPregunta<bool>> preguntasBoolean;
        private List<IPregunta<int>> preguntasInt;
        private int contadorDeFotos;

        private readonly string MENSAJE = "Responde aqui a la pregunta";

        public ViewPreguntasInspeccion(Inspeccion inspeccionRecibida, Bloque bloqueRecibido)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();
            camera = new CameraService();

            inspeccion = inspeccionRecibida;
            bloque = bloqueRecibido;
            contadorDeFotos = bloque.Fotografias.Count + 1;

            preguntasString = consult.GetPreguntasTextoByBloqueInspeccion(bloque);
            preguntasBoolean = consult.GetPreguntasBooleanByBloqueInspeccion(bloque);
            preguntasInt = consult.GetPreguntasValorByBloqueInspeccion(bloque);

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
                campoRespuesta.IsEnabled = false;
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
            aceptar.Clicked += ProcesarEditar;

            botones.Children.Add(cancelar);
            botones.Children.Add(foto);
            botones.Children.Add(aceptar);

            StackLayout verFotos = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button fotos = new Button
            {
                Text = "Ver fotografías",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            fotos.Clicked += ProcesarVerFotos;

            verFotos.Children.Add(fotos);

            layoutPreguntas.Children.Add(botones);
            layoutPreguntas.Children.Add(verFotos);
        }

        public async void ProcesarVerFotos(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewFotos(bloque)));
        }

        public void ProcesarEditar(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public async void ProcesarFotografia(object sender, EventArgs e)
        {
            try
            {
                var resultado = await camera.TakePhoto();
                var subida = resultado;

                var url = consult.UploadFoto(inspeccion.IdInspeccion.ToString(), bloque.IdBloque.ToString() + "_" + bloque.PuestoTrabajo, contadorDeFotos, subida);

                bloque.Fotografias.Add(url);

                contadorDeFotos++;
                await DisplayAlert("Correcto", "Fotografia realizada correctamente", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Se ha producido un error al realizar la foto.", "Ok");
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            List<Bloque> bloquesInspeccion = consult.GetBloquesByInspeccion(inspeccion);
            await Navigation.PushModalAsync(new NavigationPage(new ViewBloquesInspeccion(inspeccion, bloquesInspeccion)));
        }
    }
}
