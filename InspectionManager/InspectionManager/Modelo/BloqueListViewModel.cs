using System;
namespace InspectionManager.Modelo
{
    public class BloqueListViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public bool SeleccionadoAlgunaVez { get; set; }
        public string NumeroPreguntasTexto { get; set; }
        public string NumeroPreguntasBoolean { get; set; }
        public string NumeroPreguntasValor { get; set; }

        public BloqueListViewModel(string id, string nombre, int nT, int nB, int nV)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.SeleccionadoAlgunaVez = false;
            this.NumeroPreguntasTexto = "Numero de preguntas de texto: "+nT;
            this.NumeroPreguntasBoolean = "Numero de preguntas verdadero/falso: " + nB;
            this.NumeroPreguntasValor = "Numero de preguntas numericas: " + nV;
        }
    }
}
