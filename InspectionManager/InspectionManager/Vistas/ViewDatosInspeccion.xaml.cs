using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewDatosInspeccion : ContentPage
    {
        private Inspector propietario;
        private DateTime fechaInicio;
        private DateTime fechaFin;

        public ViewDatosInspeccion(Inspector inspector)
        {
            InitializeComponent();
            propietario = inspector;

            fechaInicioPicker.MinimumDate = DateTime.Today;
            fechaFinPicker.MinimumDate = fechaInicioPicker.Date.AddDays(1);
        }

        public void FechaSeleccionadaInicio(object sender, DateChangedEventArgs e)
        {
            fechaInicio = fechaInicioPicker.Date;
            fechaFinPicker.MinimumDate = fechaInicio.Date.AddDays(1);
        }

        public void FechaSeleccionadaFin(object sender, DateChangedEventArgs e)
        {
            fechaFin = fechaFinPicker.Date;
        }

        public async void ProcesarCancelarCreacion(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }

        public void ProcesarAddBloque(object sender, EventArgs e)
        {

        }

        public void ProcesarGuardarInspeccion(object sender, EventArgs e)
        {

        }
    }
}
