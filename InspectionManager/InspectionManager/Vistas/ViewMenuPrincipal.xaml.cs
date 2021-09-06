using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewMenuPrincipal : TabbedPage
    {
        private IFirebaseAuthService auth;
        private IFirebaseConsultService consult;

        public ViewMenuPrincipal()
        {
            InitializeComponent();

            auth = DependencyService.Get<IFirebaseAuthService>();
            consult = DependencyService.Get<IFirebaseConsultService>();

            string emailUsuario = auth.GetUserEmail();

            Inspector usuario = consult.GetInspectorByEmail(emailUsuario);

            dniEntry.Text = usuario.Dni;
            nombreEntry.Text = usuario.Nombre;
            apellidosEntry.Text = usuario.Apellidos;

        }

        public async void ProcesarCerrarSesion(object sender, EventArgs e)
        {
            bool logout = await auth.LogOut();

            if (logout)
            {
                await Navigation.PushAsync(new ViewLogin());
            }
            else
            {
                await DisplayAlert("Error", "No se ha podido cerrar sesion", "Ok");
            }
        }

        public void ProcesarEditarPerfil(object sender, EventArgs e)
        {

        }
    }
}
