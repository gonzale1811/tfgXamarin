using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewPregunta : ContentPage
    {
        private Bloque bloque;

        public ViewPregunta(Bloque bloqueSeleccionado)
        {
            InitializeComponent();

            bloque = bloqueSeleccionado;


        }
    }
}
