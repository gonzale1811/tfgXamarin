using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace InspectionManager.Vistas
{
    public partial class ViewMenuPrincipal : TabbedPage
    {
        private IFirebaseAuthService auth;
        private IFirebaseConsultService consult;
        private IFirebaseRetrieve<Inspector> repositoryInspectores;

        public ViewMenuPrincipal()
        {
            InitializeComponent();

            auth = DependencyService.Get<IFirebaseAuthService>();
            consult = DependencyService.Get<IFirebaseConsultService>();
            repositoryInspectores = DependencyService.Get<IFirebaseRetrieve<Inspector>>();

            string emailUsuario = auth.GetUserEmail();

            Inspector inspector = repositoryInspectores.GetOne(emailUsuario);

            dniEntry.Text = inspector.Dni;

            //Inspector usuario = consult.GetInspectorByEmail(emailUsuario);
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
