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

            Plugin.Media.CrossMedia.Current.Initialize();

            /*auth = DependencyService.Get<IFirebaseAuthService>();

            if (auth.IsUserSigned())
            {
                MainPage = new NavigationPage(new ViewMenuPrincipal());
            }
            else
            {
                MainPage = new NavigationPage(new ViewLogin());
            }*/
            MainPage = new NavigationPage(new ViewPlantillas());
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
