using System;
namespace InspectionManager.Modelo
{
    public class InspeccionListViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string NumeroDeBloques { get; set; }

        public InspeccionListViewModel(string id, string nombre, string fechaInicio, string fechaFin, int numeroDeBloques)
        {
            Id = id;
            Nombre = nombre;
            FechaInicio = "Fecha de inicio: "+fechaInicio;
            FechaFin = "Fecha de fin: " + fechaFin;
            NumeroDeBloques = "Número de bloques: "+numeroDeBloques.ToString();
        }
    }
}
