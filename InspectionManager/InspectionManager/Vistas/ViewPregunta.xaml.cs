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
        private List<IPregunta<string>> preguntasString;
        private List<IPregunta<bool>> preguntasBoolean;
        private List<IPregunta<int>> preguntasInt;
        private Bloque bloque;

        private readonly string MENSAJE = "Responde aqui a la pregunta";

        public ViewPregunta(Bloque bloqueSeleccionado)
        {
            InitializeComponent();

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

            };

            Button aceptar = new Button
            {
                Text = "Aceptar",
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            cancelar.Clicked += ProcesarCancelar;
            aceptar.Clicked += ProcesarAceptar;

            botones.Children.Add(cancelar);
            botones.Children.Add(aceptar);
            layoutPreguntas.Children.Add(botones);
        }

        public void ProcesarAceptar(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }
    }
}
