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
        private Inspeccion inspeccionCreada;
        private List<Bloque> bloques;
        private IFirebaseConsultService consult;

        public ViewBloques(Inspeccion inspeccion, Plantilla plantilla)
        {
            InitializeComponent();

            inspeccionCreada = inspeccion;

            consult = DependencyService.Get<IFirebaseConsultService>();

            this.plantilla = plantilla;
            bloques = consult.GetBloquesByPlantilla(plantilla);

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
            string idSeleccionado = ((BloqueListViewModel)((ListView)sender).SelectedItem).Id;
            foreach (Bloque b in bloques)
            {
                if (b.IdBloque.ToString() == idSeleccionado)
                {
                    await Navigation.PushAsync(new NavigationPage(new ViewPregunta(plantilla,b)));
                }
            }
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }
    }
}
