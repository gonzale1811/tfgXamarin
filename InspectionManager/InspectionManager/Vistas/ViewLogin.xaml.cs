using System;
using System.Collections.Generic;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewLogin : ContentPage
    {
        private IFirebaseAuthService auth;

        public ViewLogin()
        {
            InitializeComponent();

            auth = DependencyService.Get<IFirebaseAuthService>();
        }

        public async void ProcesarLogin(object sender, EventArgs e)
        {
            if(usernameInput.Placeholder.Equals("Correo electronico"))
            {
                usernameInput.Placeholder = "Debe introducir su correo electronico";
                usernameInput.PlaceholderColor = Color.Red;
            }else if (passwordInput.Placeholder.Equals("Contraseña"))
            {
                passwordInput.Placeholder = "Debe introducir su contraseña";
                passwordInput.PlaceholderColor = Color.Red;
            }

            string Token = await auth.SignIn(usernameInput.Text, passwordInput.Text);

            if (!Token.Equals(string.Empty))
            {
                await Navigation.PushAsync(new ViewMenuPrincipal());
            }
            else
            {
                MostrarErrorLogin("No se ha podido iniciar sesion, revise el correo electronico y la contraseña.");
            }
        }

        public async void ProcesarRegistro(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRegistrarse());
        }

        private void MostrarErrorLogin(string error)
        {
            errorLabel.Text = error;
            errorLabel.TextColor = Color.Red;
            errorLabel.IsVisible = true;
        }
    }
}
