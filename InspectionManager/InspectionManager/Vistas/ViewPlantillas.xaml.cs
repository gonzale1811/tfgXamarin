using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Inspeccion nuevaInspeccion;

        public ObservableCollection<PlantillaListViewModel> Items { get; set; }

        public ViewPlantillas(Inspeccion inspeccion)
        {
            InitializeComponent();

            nuevaInspeccion = inspeccion;

            consult = DependencyService.Get<IFirebaseConsultService>();

            plantillas = consult.GetPlantillas();
            List<PlantillaListViewModel> modelo = new List<PlantillaListViewModel>();

            foreach(Plantilla p in plantillas)
            {
                Console.WriteLine("Id de la plantilla = "+p.IdPlantilla.ToString());
                PlantillaListViewModel modeloPlantilla = new PlantillaListViewModel(p.IdPlantilla.ToString(),p.Nombre,p.VersionActual,p.Trabajo);
                modelo.Add(modeloPlantilla);
            }

            listViewPlantillas.ItemsSource = modelo;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //((ListView)sender).SelectedItem = null;
            string idSeleccionado = ((PlantillaListViewModel)((ListView)sender).SelectedItem).Id;
            foreach(Plantilla p in plantillas)
            {
                if (p.IdPlantilla.ToString() == idSeleccionado)
                {
                    await Navigation.PushAsync(new ViewBloques(nuevaInspeccion,p));
                }
            }
            
        }

        public async void ProcesarCancelar(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewMenuPrincipal()));
        }
    }
}
