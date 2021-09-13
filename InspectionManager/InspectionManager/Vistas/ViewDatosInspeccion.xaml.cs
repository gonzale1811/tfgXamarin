using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewDatosInspeccion : ContentPage
    {
        private IFirebaseConsultService consult;
        private Inspector propietario;
        private DateTime fechaInicio;
        private DateTime fechaFin;

        public ViewDatosInspeccion(Inspector inspector)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            propietario = inspector;

            fechaInicioPicker.MinimumDate = DateTime.Today;
            fechaInicio = DateTime.Today;
            fechaFinPicker.MinimumDate = fechaInicioPicker.Date.AddDays(1);
            fechaFin = fechaInicioPicker.Date.AddDays(1);
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

        public async void ProcesarAddBloque(object sender, EventArgs e)
        {
            ocultarError();

            if (comprobarCampos())
            {
                Direccion direccionInspeccion;
                if (String.IsNullOrWhiteSpace(numeroEntry.Text))
                {
                    direccionInspeccion = new Direccion(calleEntry.Text, "Sin numero", localidadEntry.Text, codigoPostalEntry.Text);
                }
                else
                {
                    direccionInspeccion = new Direccion(calleEntry.Text, numeroEntry.Text, localidadEntry.Text, codigoPostalEntry.Text);
                }

                Inspeccion nuevaInspeccion = new Inspeccion(nombreEntry.Text, fechaInicio, fechaFin, direccionInspeccion);
                propietario.Inspecciones.Add(nuevaInspeccion.IdInspeccion.ToString());
                consult.AddInspeccion(nuevaInspeccion);
                consult.AddInspeccionToInspector(propietario, nuevaInspeccion);
                await Navigation.PushModalAsync(new NavigationPage(new ViewPlantillas(nuevaInspeccion)));
            }
            else
            {
                await DisplayAlert("Error", "Alguno de los campos no ha sido cumplimentado o es erroneo.", "Ok");
            }
        }

        private bool comprobarCampos()
        {
            if (String.IsNullOrWhiteSpace(nombreEntry.Text))
            {
                mostrarError("El campo nombre no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(calleEntry.Text))
            {
                mostrarError("El campo calle no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(localidadEntry.Text))
            {
                mostrarError("El campo localidad no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(codigoPostalEntry.Text))
            {
                mostrarError("El campo código postal no puede estar vacío.");
                return false;
            }
            if(codigoPostalEntry.Text.Length != 5)
            {
                mostrarError("El código postal debe tener una longitud de 5 dígitos.");
                return false;
            }

            return true;
        }

        private void mostrarError(string error)
        {
            errorLabel.Text = error;
            errorLabel.TextColor = Color.Red;
            errorLabel.IsVisible = true;
        }

        private void ocultarError()
        {
            errorLabel.IsVisible = false;
        }
    }
}
