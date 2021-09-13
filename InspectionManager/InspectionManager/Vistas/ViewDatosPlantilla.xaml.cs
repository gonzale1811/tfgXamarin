using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewDatosPlantilla : ContentPage
    {
        private Plantilla plantillaCreada;
        private Plantilla plantillaRecibida;
        private List<Bloque> bloquesCreados;

        private IFirebaseConsultService consult;

        public ViewDatosPlantilla(Plantilla plantilla, List<Bloque> bloques)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            plantillaRecibida = plantilla;

            if (bloques == null)
            {
                bloquesCreados = new List<Bloque>();
            }
            else
            {
                bloquesCreados = bloques;
                if (bloquesCreados.Count > 0)
                {
                    finalizarButton.IsEnabled = true;
                }
            }

            List<TipoTrabajo> itemsPicker = new List<TipoTrabajo> { TipoTrabajo.Obra, TipoTrabajo.Oficina,
                TipoTrabajo.Fabrica, TipoTrabajo.Servicios };

            tipoTrabajoPicker.Title = "Seleccione el tipo de la " +
                "plantilla.";
            tipoTrabajoPicker.ItemsSource = itemsPicker;

            if (plantillaRecibida != null)
            {
                nombreEntry.Text = plantillaRecibida.Nombre;
                tipoTrabajoPicker.SelectedItem = plantillaRecibida.Trabajo;
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            bool cancelar = await DisplayAlert("Precaución", "Si cancela el proceso se eliminarán los datos guardados, ¿desea continuar?", "Si", "No");

            if (cancelar)
            {
                consult.CancelarCreacionPlantilla(plantillaRecibida, bloquesCreados);
                await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
            }
        }

        public async void ProcesarAddBloque(object sender, EventArgs e)
        {
            OcultarError();

            if (ComprobarCampos())
            {
                if (plantillaRecibida == null)
                {
                    plantillaCreada = new Plantilla(nombreEntry.Text, (TipoTrabajo)tipoTrabajoPicker.SelectedItem);
                    consult.AddPlantilla(plantillaCreada);
                }
                else
                {
                    plantillaCreada = plantillaRecibida;
                }

                await Navigation.PushAsync(new ViewAddBloque(plantillaCreada, bloquesCreados));
            }
            else
            {
                await DisplayAlert("Error", "Alguno de los campos es incorrecto o esta vacio.", "Ok");
            }
        }

        public async void ProcesarFinalizar(object sender, EventArgs e)
        {
            consult.SetBloquesToPlantilla(plantillaRecibida);
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }

        private bool ComprobarCampos()
        {
            if (String.IsNullOrWhiteSpace(nombreEntry.Text))
            {
                MostrarError("El campo nombre no puede estar vacio.");
                return false;
            }
            if (tipoTrabajoPicker.SelectedItem == null)
            {
                MostrarError("Debe seleccionar un tipo de trabajo.");
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
