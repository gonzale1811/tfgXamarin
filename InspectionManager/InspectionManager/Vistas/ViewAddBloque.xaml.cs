using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewAddBloque : ContentPage
    {
        private List<string> itemsPicker;

        private IFirebaseConsultService consult;

        private Plantilla plantillaCreada;
        private List<Bloque> bloquesCreados;

        private List<IPregunta<string>> preguntasTextoCreadas;
        private List<IPregunta<bool>> preguntasBooleanCreadas;
        private List<IPregunta<int>> preguntasValorCreadas;

        private Bloque bloqueCreado;
        private IPregunta<string> preguntaTextoCreada;
        private IPregunta<bool> preguntaBooleanCreada;
        private IPregunta<int> preguntaValorCreada;

        public ViewAddBloque(Plantilla plantilla, List<Bloque> bloques)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            plantillaCreada = plantilla;
            bloquesCreados = bloques;

            itemsPicker = new List<string> { "Pregunta de Texto", "Pregunta Verdadero/Falso", "Pregunta Numérica" };

            tipoPreguntaPicker.ItemsSource = itemsPicker;
        }

        public async void ProcesarCrearBloque(object sender, EventArgs e)
        {
            OcultarError();

            if (ComprobarInformacionBloque())
            {
                if (bloqueCreado != null)
                {
                    if (bloqueCreado.Nombre != nombreBloqueEntry.Text)
                    {
                        bloqueCreado.Nombre = nombreBloqueEntry.Text;
                    }
                }
                else
                {
                    bloqueCreado = new Bloque(nombreBloqueEntry.Text);
                    preguntasTextoCreadas = new List<IPregunta<string>>();
                    preguntasBooleanCreadas = new List<IPregunta<bool>>();
                    preguntasValorCreadas = new List<IPregunta<int>>();
                }

                enunciadoPreguntaEntry.IsEnabled = true;
                tipoPreguntaPicker.IsEnabled = true;
                crearPregunta.IsEnabled = true;
                nombreBloqueEntry.IsEnabled = false;
                crearBloqueButton.IsVisible = false;
                crearBloqueButton.IsEnabled = false;
                cambiarNombreBloque.IsVisible = true;
                cambiarNombreBloque.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error", "El nombre del bloque no puede estar vacio.", "Ok");
            }
        }

        public void ProcesarEditarBloque(object sender, EventArgs e)
        {
            tipoPreguntaPicker.SelectedItem = null;
            tipoPreguntaPicker.IsEnabled = false;
            enunciadoPreguntaEntry.Text = null;
            enunciadoPreguntaEntry.IsEnabled = false;
            crearPregunta.IsEnabled = false;
            cambiarNombreBloque.IsVisible = false;
            cambiarNombreBloque.IsEnabled = false;
            crearBloqueButton.IsEnabled = true;
            crearBloqueButton.IsVisible = true;
            nombreBloqueEntry.IsEnabled = true;
        }

        public async void ProcesarCrearPregunta(object sender, EventArgs e)
        {
            OcultarError();

            if (ComprobarCamposPregunta())
            {
                if ((string)tipoPreguntaPicker.SelectedItem == itemsPicker[0])
                {
                    preguntaTextoCreada = new PreguntaTexto(enunciadoPreguntaEntry.Text);
                    bloqueCreado.AddPreguntaTexto(preguntaTextoCreada.IdPregunta.ToString());
                    preguntasTextoCreadas.Add(preguntaTextoCreada);
                }else if ((string)tipoPreguntaPicker.SelectedItem == itemsPicker[1])
                {
                    preguntaBooleanCreada = new PreguntaBoolean(enunciadoPreguntaEntry.Text);
                    bloqueCreado.AddPreguntaBoolean(preguntaBooleanCreada.IdPregunta.ToString());
                    preguntasBooleanCreadas.Add(preguntaBooleanCreada);
                }else if ((string)tipoPreguntaPicker.SelectedItem == itemsPicker[2])
                {
                    preguntaValorCreada = new PreguntaValor(enunciadoPreguntaEntry.Text);
                    bloqueCreado.AddPreguntaValor(preguntaValorCreada.IdPregunta.ToString());
                    preguntasValorCreadas.Add(preguntaValorCreada);
                }

                enunciadoPreguntaEntry.Text = null;
                tipoPreguntaPicker.SelectedItem = null;
                guardarButton.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error", "No se ha cumplimentado alguno de los campos.", "Ok");
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            consult.CancelarCreacionPlantilla(plantillaCreada, bloquesCreados);
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }

        public async void ProcesarGuardar(object sender, EventArgs e)
        {
            consult.AddBloquePlantilla(bloqueCreado);
            bloquesCreados.Add(bloqueCreado);
            plantillaCreada.AddBloque(bloqueCreado.IdBloque.ToString());
            consult.AddPreguntasTexto(preguntasTextoCreadas, "PreguntasTexto", false);
            consult.AddPreguntasBoolean(preguntasBooleanCreadas, "PreguntasBoolean", false);
            consult.AddPreguntasValor(preguntasValorCreadas, "PreguntasValor", false);

            await Navigation.PushModalAsync(new NavigationPage(new ViewDatosPlantilla(plantillaCreada, bloquesCreados)));
        }

        private bool ComprobarCamposPregunta()
        {
            if (String.IsNullOrWhiteSpace(enunciadoPreguntaEntry.Text))
            {
                MostrarError("El enunciado de la pregunta no puede estar vacío.");
                return false;
            }
            if (tipoPreguntaPicker.SelectedItem == null)
            {
                MostrarError("Debe seleccionar el tipo de pregunta.");
                return false;
            }
            if (!ComprobarInformacionBloque())
            {
                MostrarError("Ha eliminado el nombre del bloque, por favor rellene el campo.");
                return false;
            }

            return true;
        }

        private bool ComprobarInformacionBloque()
        {
            if (String.IsNullOrWhiteSpace(nombreBloqueEntry.Text))
            {
                MostrarError("El nombre del bloque no puede estar vacío.");
                return false;
            }

            return true;
        }

        private void MostrarError(string error)
        {
            errorLabel.Text = error;
            errorLabel.TextColor = Color.Red;
            errorLabel.IsVisible = true;
        }

        private void OcultarError()
        {
            errorLabel.IsVisible = false;
        }
    }
}
