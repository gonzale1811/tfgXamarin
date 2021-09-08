using System;
using System.Collections.Generic;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

namespace InspectionManager.Vistas
{
    public partial class ViewPregunta : ContentPage
    {
        private IFirebaseConsultService consult;
        private List<IPregunta<string>> preguntasString;
        private List<IPregunta<bool>> preguntasBoolean;
        private List<IPregunta<int>> preguntasInt;
        private Bloque bloque;

        public ViewPregunta(Bloque bloqueSeleccionado)
        {
            InitializeComponent();

            consult = DependencyService.Get<IFirebaseConsultService>();

            preguntasString = consult.GetPreguntasTextoByBloque(bloqueSeleccionado);
            preguntasBoolean = consult.GetPreguntasBooleanByBloque(bloqueSeleccionado);
            preguntasInt = consult.GetPreguntasValorByBloque(bloqueSeleccionado);

            Console.WriteLine("Numero de texto = " + preguntasString.Count + ", numero boolean = " + preguntasBoolean.Count + " y numero valor = " + preguntasInt.Count);
        }
    }
}
