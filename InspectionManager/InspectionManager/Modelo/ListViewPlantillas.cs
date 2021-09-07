using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace InspectionManager.Modelo
{
    public class ListViewPlantillas
    {
        public ListViewPlantillas()
        {
        }

        public ObservableCollection<Button> ObtenerListaPlantillas(List<Plantilla> lista)
        {
            ObservableCollection<Button> listaPlantillas = new ObservableCollection<Button>();

            foreach(Plantilla p in lista)
            {
                listaPlantillas.Add(new Button
                {
                    Text = p.Nombre + ", Version = " + p.VersionActual,
                    ImageSource = seleccionarIcono(p.TrabajoToString())
                });
            }

            return listaPlantillas;
        }

        private string seleccionarIcono(string trabajo)
        {
            switch (trabajo)
            {
                case "Oficina":
                    return "oficina_black.png";
                case "Obra":
                    return "obra_black.png";
                default:
                    return null;
            }
        }
    }
}
