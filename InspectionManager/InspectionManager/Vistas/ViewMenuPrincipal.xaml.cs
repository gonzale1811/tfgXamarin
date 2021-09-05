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

            List<Inspector> usuarios = consult.GetInspectores();

            string emailUsuario = auth.GetUserEmail();

            Inspector usuarioActual;

            foreach(Inspector i in usuarios)
            {
                if (i.Usuario == emailUsuario)
                {
                    usuarioActual = i;
                    dniEntry.Text = usuarioActual.Dni;
                }
            }
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

        public void ProcesarEditarPerfil(object sender, EventArgs e)
        {

        }
    }
}
