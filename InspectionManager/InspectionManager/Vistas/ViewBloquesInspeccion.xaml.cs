using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using InspectionManager.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InspectionManager.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewBloquesInspeccion : ContentPage
    {
        private Inspeccion inspeccion;
        private List<Bloque> bloques;
        private List<BloqueListViewModel> items;

        public ViewBloquesInspeccion(Inspeccion inspeccionRecibida, List<Bloque> bloquesInspeccion)
        {
            InitializeComponent();

            inspeccion = inspeccionRecibida;
            bloques = bloquesInspeccion;

            items = new List<BloqueListViewModel>();

            foreach (Bloque b in bloques)
            {
                BloqueListViewModel nuevo = new BloqueListViewModel(b.IdBloque.ToString(), b.Nombre, b.PreguntasTexto.Count, b.PreguntasBoolean.Count, b.PreguntasValor.Count, b.PuestoTrabajo);
                items.Add(nuevo);
            }

            MyListView.ItemsSource = items;
        }

        public async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string idSeleccionado = ((BloqueListViewModel)((ListView)sender).SelectedItem).Id;
            foreach (Bloque b in bloques)
            {
                if (b.IdBloque.ToString() == idSeleccionado)
                {
                    await Navigation.PushModalAsync(new NavigationPage(new ViewPreguntasInspeccion(inspeccion,b)));
                }
            }
        }
    }
}
