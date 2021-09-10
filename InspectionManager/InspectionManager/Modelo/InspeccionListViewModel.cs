using System;
namespace InspectionManager.Modelo
{
    public class InspeccionListViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int NumeroDeBloques { get; set; }

        public InspeccionListViewModel(string id, string nombre, string fechaInicio, string fechaFin, int numeroDeBloques)
        {
            Id = id;
            Nombre = nombre;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            NumeroDeBloques = numeroDeBloques;
        }
    }
}
