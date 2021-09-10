using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewDatosPlantilla : ContentPage
    {
        public ViewDatosPlantilla()
        {
            InitializeComponent();

            List<TipoTrabajo> itemsPicker = new List<TipoTrabajo> { TipoTrabajo.Obra, TipoTrabajo.Oficina,
                TipoTrabajo.Fabrica, TipoTrabajo.Servicios };

            tipoTrabajoPicker.Title = "Seleccione el tipo de la " +
                "plantilla.";
            tipoTrabajoPicker.ItemsSource = itemsPicker;
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }

        public async void ProcesarAddBloque(object sender, EventArgs e)
        {
            if (ComprobarCampos())
            {
                Plantilla plantillaCreada = new Plantilla(nombreEntry.Text, (TipoTrabajo)tipoTrabajoPicker.SelectedItem);
                await Navigation.PushAsync(new NavigationPage(new ViewAddBloque(plantillaCreada)));
            }
        }

        public void ProcesarFinalizar(object sender, EventArgs e)
        {

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
    }
}
