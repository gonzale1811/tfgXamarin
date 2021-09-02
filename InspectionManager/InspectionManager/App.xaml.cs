using System;
using InspectionManager.Servicios;
using InspectionManager.Vistas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InspectionManager
{
    public partial class App : Application
    {
        private IFirebaseAuthService auth;

        public App()
        {
            InitializeComponent();

            auth = DependencyService.Get<IFirebaseAuthService>();

            if (auth.IsUserSigned())
            {
                MainPage = new ViewMenuPrincipal();
            }
            else
            {
                MainPage = new ViewLogin();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
