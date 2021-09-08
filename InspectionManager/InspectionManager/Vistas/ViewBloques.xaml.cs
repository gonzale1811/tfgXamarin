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
    public partial class ViewBloques : ContentPage
    {
        private Plantilla plantilla;
        private List<Bloque> bloques;
        private IFirebaseConsultService consult;

        public ViewBloques(Plantilla plantilla)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            this.plantilla = plantilla;
            bloques = consult.GetBloquesByPlantilla(this.plantilla);

            List<BloqueListViewModel> items = new List<BloqueListViewModel>();

            foreach(Bloque b in bloques)
            {
                BloqueListViewModel nuevo = new BloqueListViewModel(b.IdBloque.ToString(), b.Nombre, b.PreguntasTexto.Count, b.PreguntasBoolean.Count, b.PreguntasValor.Count);
                items.Add(nuevo);
            }

            MyListView.ItemsSource = items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }
    }
}
