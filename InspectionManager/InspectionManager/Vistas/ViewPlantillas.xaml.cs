using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InspectionManager.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPlantillas : ContentPage
    {
        private IFirebaseConsultService consult;
        private List<Plantilla> plantillas;

        public ObservableCollection<PlantillaListViewModel> Items { get; set; }

        public ViewPlantillas()
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            plantillas = consult.GetPlantillas();
            List<PlantillaListViewModel> modelo = new List<PlantillaListViewModel>();

            foreach(Plantilla p in plantillas)
            {
                PlantillaListViewModel modeloPlantilla = new PlantillaListViewModel(p.IdPlantilla.ToString(),p.Nombre,p.VersionActual,p.Trabajo);
                modelo.Add(modeloPlantilla);
            }

            listViewPlantillas.ItemsSource = modelo;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
            //((PlantillaListViewModel)((ListView)sender).SelectedItem)
        }
    }
}
