using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewInformacionInspeccion : ContentPage
    {
        private Inspector usuario;
        private Inspeccion inspeccion;
        private List<Bloque> bloquesInspeccion;
        private IFirebaseConsultService consult;
        private DateTime nuevaFechaInicio;
        private DateTime nuevaFechaFin;

        public ViewInformacionInspeccion(Inspector inspector, Inspeccion inspeccionElegida)
        {
            InitializeComponent();

            usuario = inspector;
            inspeccion = inspeccionElegida;

            consult = DependencyService.Get<IFirebaseConsultService>();

            bloquesInspeccion = consult.GetBloquesByInspeccion(inspeccion);

            nombreEntry.Text = "Nombre: "+inspeccion.Nombre;
            fechaInicioPicker.Date = DateTime.ParseExact(inspeccion.FechaInicio,"dd/MM/yyyy",null);
            fechaInicioPicker.MinimumDate = DateTime.ParseExact(inspeccion.FechaInicio, "dd/MM/yyyy", null);
            fechaInicioPicker.MaximumDate = DateTime.ParseExact(inspeccion.FechaInicio, "dd/MM/yyyy", null);
            fechaFinPicker.Date = DateTime.ParseExact(inspeccion.FechaFin, "dd/MM/yyyy", null);
            fechaFinPicker.MinimumDate = DateTime.ParseExact(inspeccion.FechaFin, "dd/MM/yyyy", null);
            fechaFinPicker.MaximumDate = DateTime.ParseExact(inspeccion.FechaFin, "dd/MM/yyyy", null);
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
                int index = usuario.Inspecciones.IndexOf(inspeccion.IdInspeccion.ToString());
                usuario.Inspecciones.RemoveAt(index);
                consult.DeleteInspeccion(usuario, inspeccion, bloquesInspeccion);
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
            fechaInicioPicker.MinimumDate = DateTime.ParseExact(inspeccion.FechaInicio, "dd/MM/yyyy", null);
            fechaInicioPicker.MaximumDate = new DateTime(3000, 1, 1);
            nuevaFechaInicio = fechaInicioPicker.Date;
            fechaFinPicker.MinimumDate = DateTime.ParseExact(inspeccion.FechaInicio, "dd/MM/yyyy", null).AddDays(1);
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
                Inspeccion nuevaInspeccion = new Inspeccion(inspeccion.IdInspeccion, nombreEntry.Text, nuevaFechaInicio.ToString("dd/MM/yyyy"), nuevaFechaFin.ToString("dd/MM/yyyy"));
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
                fechaInicioPicker.MinimumDate = DateTime.ParseExact(inspeccion.FechaInicio, "dd/MM/yyyy", null);
                fechaInicioPicker.MaximumDate = DateTime.ParseExact(inspeccion.FechaInicio, "dd/MM/yyyy", null);
                fechaFinPicker.MinimumDate = DateTime.ParseExact(inspeccion.FechaFin, "dd/MM/yyyy", null);
                fechaFinPicker.MaximumDate = DateTime.ParseExact(inspeccion.FechaFin, "dd/MM/yyyy", null);
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
