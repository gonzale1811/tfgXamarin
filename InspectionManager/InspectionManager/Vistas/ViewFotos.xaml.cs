using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewFotos : ContentPage
    {
        private IFirebaseConsultService consult;

        private Bloque bloque;
        private Inspeccion inspeccion;

        public ViewFotos(Inspeccion inspeccionRecibida, Bloque bloqueRecibido)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            bloque = bloqueRecibido;
            inspeccion = inspeccionRecibida;

            List<string> opciones = new List<string>();
            int cont = 1;

            foreach(string foto in bloque.Fotografias)
            {
                opciones.Add("Evidencia " + cont);
                cont++;
            }

            fotosPicker.ItemsSource = opciones;
        }

        public async void ProcesarVolver(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ViewPreguntasInspeccion(inspeccion,bloque)));
        }

        public async void ProcesarCargarFoto(object sender, EventArgs e)
        {
            if (fotosPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Debe seleccionar una foto para mostrar.", "Ok");
            }
            else
            {
                int nombre = fotosPicker.SelectedIndex + 1;
                string uri = await consult.DonwloadImage(inspeccion.IdInspeccion.ToString(), bloque.IdBloque.ToString()+"_"+bloque.PuestoTrabajo,"evidencia"+nombre+".png");
                Console.WriteLine("INDEX DE LA IMAGEN OBTENIDO: " + uri);
                fotoImage.Source = Xamarin.Forms.ImageSource.FromUri(new Uri(uri));
            }
        }
    }
}
