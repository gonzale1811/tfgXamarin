using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewInformacionInspeccion : ContentPage
    {
        private Inspeccion inspeccion;
        private List<Bloque> bloquesInspeccion;
        private IFirebaseConsultService consult;
        private DateTime nuevaFechaInicio;
        private DateTime nuevaFechaFin;

        public ViewInformacionInspeccion(Inspeccion inspeccionElegida)
        {
            InitializeComponent();

            inspeccion = inspeccionElegida;

            consult = DependencyService.Get<IFirebaseConsultService>();

            bloquesInspeccion = consult.GetBloquesByInspeccion(inspeccion);

            nombreEntry.Text = "Nombre: "+inspeccion.Nombre;
            fechaInicioPicker.Date = inspeccion.FechaInicio;
            fechaInicioPicker.MinimumDate = inspeccion.FechaInicio;
            fechaInicioPicker.MaximumDate = inspeccion.FechaInicio;
            fechaFinPicker.Date = inspeccion.FechaFin;
            fechaFinPicker.MinimumDate = inspeccion.FechaFin;
            fechaFinPicker.MaximumDate = inspeccion.FechaFin;
            calleEntry.Text = "Calle: "+inspeccion.DireccionInspeccion.Calle;
            numeroEntry.Text = "Número: "+inspeccion.DireccionInspeccion.Numero;
            localidadEntry.Text = "Localidad: "+inspeccion.DireccionInspeccion.Localidad;
            codigoPostalEntry.Text = "Código Postal: "+inspeccion.DireccionInspeccion.CodigoPostal;
        }

        public async void ProcesarEliminarInspeccion(object sender, EventArgs e)
        {
            bool eliminar = await DisplayAlert("Precaución", "Eliminar la inspección es una acción permanente, ¿desea continuar?", "Si", "No");
            if (eliminar)
            {
                consult.DeleteInspeccion(inspeccion, bloquesInspeccion);
                await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
            }
        }

        public async void ProcesarDescargarInspeccion(object sender, EventArgs e)
        {
            consult.GenerarPdfInspeccion(inspeccion, bloquesInspeccion);
            await DisplayAlert("Correcto", "Archivo pdf generado en la carpeta InspectionManager", "Ok");
        }

        public void ProcesarEditarInspeccion(object sender, EventArgs e)
        {
            nombreEntry.Text = inspeccion.Nombre;
            nombreEntry.IsReadOnly = false;
            fechaInicioPicker.MinimumDate = inspeccion.FechaInicio;
            fechaInicioPicker.MaximumDate = new DateTime(3000, 1, 1);
            nuevaFechaInicio = fechaInicioPicker.Date;
            fechaFinPicker.MinimumDate = inspeccion.FechaInicio.AddDays(1);
            fechaFinPicker.MaximumDate = new DateTime(3000, 1, 1);
            nuevaFechaFin = fechaFinPicker.Date;
            calleEntry.Text = inspeccion.DireccionInspeccion.Calle;
            calleEntry.IsReadOnly = false;
            numeroEntry.Text = inspeccion.DireccionInspeccion.Numero;
            numeroEntry.IsReadOnly = false;
            localidadEntry.Text = inspeccion.DireccionInspeccion.Localidad;
            localidadEntry.IsReadOnly = false;
            codigoPostalEntry.Text = inspeccion.DireccionInspeccion.CodigoPostal;
            codigoPostalEntry.IsReadOnly = false;

            editarButton.IsEnabled = false;
            editarButton.IsVisible = false;

            guardarNuevosDatosButton.IsVisible = true;
            guardarNuevosDatosButton.IsEnabled = true;
        }

        public async void ProcesarObtenerBloques(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewBloquesInspeccion(inspeccion, bloquesInspeccion));
        }

        public async void ProcesarGuardarNuevosDatos(object sender, EventArgs e)
        {
            OcultarError();

            if (ComprobarCampos())
            {
                Inspeccion nuevaInspeccion = new Inspeccion(inspeccion.IdInspeccion, nombreEntry.Text, nuevaFechaInicio, nuevaFechaFin);
                nuevaInspeccion.DireccionInspeccion.Calle = calleEntry.Text;

                if (String.IsNullOrWhiteSpace(numeroEntry.Text))
                {
                    nuevaInspeccion.DireccionInspeccion.Numero = "Sin numero";
                }
                else
                {
                    nuevaInspeccion.DireccionInspeccion.Numero = numeroEntry.Text;
                }

                nuevaInspeccion.DireccionInspeccion.Localidad = localidadEntry.Text;
                nuevaInspeccion.DireccionInspeccion.CodigoPostal = codigoPostalEntry.Text;

                consult.ActualizarInspeccion(nuevaInspeccion);

                inspeccion.Nombre = nuevaInspeccion.Nombre;
                inspeccion.FechaInicio = nuevaInspeccion.FechaInicio;
                inspeccion.FechaFin = nuevaInspeccion.FechaFin;
                inspeccion.DireccionInspeccion = nuevaInspeccion.DireccionInspeccion;

                nombreEntry.Text = "Nombre: " + nuevaInspeccion.Nombre;
                nombreEntry.IsReadOnly = true;
                fechaInicioPicker.MinimumDate = inspeccion.FechaInicio;
                fechaInicioPicker.MaximumDate = inspeccion.FechaInicio;
                fechaFinPicker.MinimumDate = inspeccion.FechaFin;
                fechaFinPicker.MaximumDate = inspeccion.FechaFin;
                calleEntry.Text = "Calle: " + nuevaInspeccion.DireccionInspeccion.Calle;
                calleEntry.IsReadOnly = true;
                numeroEntry.Text = "Número: " + nuevaInspeccion.DireccionInspeccion.Numero;
                numeroEntry.IsReadOnly = true;
                localidadEntry.Text = "Localidad: " + nuevaInspeccion.DireccionInspeccion.Localidad;
                localidadEntry.IsReadOnly = true;
                codigoPostalEntry.Text = "Código postal: " + nuevaInspeccion.DireccionInspeccion.CodigoPostal;
                codigoPostalEntry.IsReadOnly = true;

                guardarNuevosDatosButton.IsEnabled = false;
                guardarNuevosDatosButton.IsVisible = false;

                editarButton.IsVisible = true;
                editarButton.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error", "Alguno de los campos es erroneo o esta vacío.", "Ok");
            }
        }

        public void ProcesarFechaInicio(object sender, DateChangedEventArgs e)
        {
            nuevaFechaInicio = fechaInicioPicker.Date;
            fechaFinPicker.MinimumDate = nuevaFechaInicio.AddDays(1);
        }

        public void ProcesarFechaFin(object sender, DateChangedEventArgs e)
        {
            nuevaFechaFin = fechaFinPicker.Date;
        }

        private bool ComprobarCampos()
        {
            if (String.IsNullOrWhiteSpace(nombreEntry.Text))
            {
                MostrarError("El campo nombre no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(calleEntry.Text))
            {
                MostrarError("El campo calle no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(localidadEntry.Text))
            {
                MostrarError("El campo localidad no puede estar vacío.");
                return false;
            }
            if (String.IsNullOrWhiteSpace(codigoPostalEntry.Text))
            {
                MostrarError("El campo código postal no puede estar vacío.");
                return false;
            }
            if (codigoPostalEntry.Text.Length != 5)
            {
                MostrarError("El código postal debe estar formado por 5 números.");
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
