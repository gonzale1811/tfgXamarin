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

        public ObservableCollection<Button> Items { get; set; }

        public ViewPlantillas()
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            plantillas = consult.GetPlantillas();
        }

        protected override async void OnAppearing()
        {
            LlenarLista();
            await Task.Yield();
        }

        public async void LlenarLista()
        {
            ListViewPlantillas lista = new ListViewPlantillas();
            listViewPlantillas.ItemsSource = null;
            listViewPlantillas.ItemsSource = lista.ObtenerListaPlantillas(plantillas);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
