using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewFotos : ContentPage
    {
        private Bloque bloque;

        public ViewFotos(Bloque bloqueRecibido)
        {
            InitializeComponent();

            bloque = bloqueRecibido;

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
            await Navigation.PushModalAsync(new NavigationPage(new ViewPreguntasInspeccion(bloque)));
        }

        public async void ProcesarCargarFoto(object sender, EventArgs e)
        {
            if (fotosPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Debe seleccionar una foto para mostrar.", "Ok");
            }
            else
            {
                string imagen = fotosPicker.SelectedItem.ToString();
                int imagenSeleccionada =  Convert.ToInt32(imagen.Substring(imagen.Length - 2, imagen.Length - 1));
                fotoImage.Source = Xamarin.Forms.ImageSource.FromUri(new Uri(bloque.Fotografias[imagenSeleccionada - 1]));
            }
        }
    }
}
