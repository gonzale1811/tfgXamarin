using System;
using InspectionManager.Vistas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InspectionManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ViewLogin());
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
