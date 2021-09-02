using System;
using System.Collections.Generic;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewMenuPrincipal : ContentPage
    {
        private IFirebaseAuthService auth;

        public ViewMenuPrincipal()
        {
            InitializeComponent();

            auth = DependencyService.Get<IFirebaseAuthService>();
        }

        public async void ProcesarCerrarSesion(object sender, EventArgs e)
        {
            bool logout = await auth.LogOut();

            if (logout)
            {
                App.Current.MainPage = new ViewLogin();
            }
            else
            {
                await DisplayAlert("Error", "No se ha podido cerrar sesion", "Ok");
            }
        }
    }
}
