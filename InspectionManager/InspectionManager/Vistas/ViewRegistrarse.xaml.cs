using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewRegistrarse : ContentPage
    {
        private IFirebaseAuthService auth;
        private IFirebaseConsultService consult;
        private DateTime fechaNacimiento;
        private bool menorEdad;

        public ViewRegistrarse()
        {
            InitializeComponent();

            fechaNacimientoPicker.MaximumDate = DateTime.Today;

            auth = DependencyService.Get<IFirebaseAuthService>();
            consult = DependencyService.Get<IFirebaseConsultService>();

            fechaNacimiento = DateTime.Today;
            menorEdad = true;
        }

        public async void ProcesarRegistroUsuario(object sender, EventArgs e)
        {
            ocultarMensaje();

            bool datosValidados = comprobarCampos();

            if (datosValidados && !menorEdad)
            {
                Inspector inspector = new Inspector(dniEntry.Text, nombreEntry.Text, apellidosEntry.Text, usernameEntry.Text, passwordEntry.Text, fechaNacimiento);

                bool registro = await auth.SignUp(usernameEntry.Text, passwordEntry.Text);

                if (registro)
                {

                }
                else
                {
                    await DisplayAlert("Problema en el registro", "No se ha podido registrar el nuevo usuario, revise su conexión.", "Ok");
                }
            }
        }

        void FechaSeleccionada(object sender, DateChangedEventArgs e)
        {

            mostrarError(fechaNacimientoPicker.Date.ToString(), Color.Green);

            if (fechaNacimientoPicker.Date.AddYears(18) <= DateTime.Today)
            {
                fechaNacimiento = fechaNacimientoPicker.Date;
                menorEdad = false;
            }
            else
            {
                mostrarError("Fecha de nacimiento no válida, debe ser mayor de 18 años.", Color.Red);
                menorEdad = true;
            }
        }

        private bool comprobarCampos()
        {
            if (String.IsNullOrWhiteSpace(dniEntry.Text))
            {
                mostrarError("El campo DNI no puede estar vacío.", Color.Red);
                return false;
            }
            if (String.IsNullOrWhiteSpace(nombreEntry.Text))
            {
                mostrarError("El campo Nombre no puede estar vacío.", Color.Red);
                return false;
            }
            if (String.IsNullOrWhiteSpace(apellidosEntry.Text))
            {
                mostrarError("El campo Apellidos no puede estar vació.", Color.Red);
                return false;
            }
            if (String.IsNullOrWhiteSpace(usernameEntry.Text))
            {
                mostrarError("El campo Correo electrónico no puede estar vacío.", Color.Red);
                return false;
            }
            if (String.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                mostrarError("El campo Contraseña no puede estar vacío.", Color.Red);
                return false;
            }
            if (String.IsNullOrWhiteSpace(passwordConfirmEntry.Text))
            {
                mostrarError("El campo de Confirmación de Contraseña no puede estar vacío.", Color.Red);
                return false;
            }
            if (!String.IsNullOrWhiteSpace(passwordEntry.Text)&&!String.IsNullOrWhiteSpace(passwordConfirmEntry.Text)&&!passwordEntry.Text.Equals(passwordConfirmEntry.Text))
            {
                mostrarError("Las contraseñas deben de ser iguales", Color.Red);
                
            }

            return true;
        }

        private void mostrarError(string error, Color color)
        {
            labelError.Text = error;
            labelError.TextColor = color;
            labelError.IsVisible = true;
        }

        private void ocultarMensaje()
        {
            labelError.IsVisible = false;
        }
    }
}
