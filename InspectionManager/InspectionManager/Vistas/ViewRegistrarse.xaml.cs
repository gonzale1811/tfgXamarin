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
        private DateTime fechaNacimiento;
        private bool menorEdad;

        public ViewRegistrarse()
        {
            InitializeComponent();

            fechaNacimientoPicker.MaximumDate = DateTime.Today;

            auth = DependencyService.Get<IFirebaseAuthService>();
            fechaNacimiento = DateTime.Today;
            menorEdad = true;
        }

        public void ProcesarRegistroUsuario(object sender, EventArgs e)
        {
            bool datosValidados = comprobarCampos();

            if (datosValidados)
            {
                mostrarError("Datos válidos.", Color.Blue);
                Inspector inspector = new Inspector(dniEntry.Text, nombreEntry.Text, apellidosEntry.Text, usernameEntry.Text, passwordEntry.Text, fechaNacimiento);
            }
            else
            {
                mostrarError("Error en los campos.", Color.Red);
            }
        }

        void FechaSeleccionada(object sender, DateChangedEventArgs e)
        {
            if (fechaNacimientoPicker.Date.AddYears(18) < DateTime.Today)
            {
                fechaNacimiento = fechaNacimientoPicker.Date;
                menorEdad = false;
            }
            else
            {
                menorEdad = true;
            }
        }

        private bool comprobarCampos()
        {
            if (String.IsNullOrWhiteSpace(dniEntry.Text))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(nombreEntry.Text))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(apellidosEntry.Text))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(usernameEntry.Text))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(passwordConfirmEntry.Text))
            {
                return false;
            }
            if (!String.IsNullOrWhiteSpace(passwordEntry.Text)&&!String.IsNullOrWhiteSpace(passwordConfirmEntry.Text)&&!passwordEntry.Text.Equals(passwordConfirmEntry.Text))
            {
                mostrarError("Las contraseñas deben de ser iguales", Color.Red);
                
            }
            if (menorEdad)
            {
                return false;
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
