using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Inspeccion
    {
        public Guid IdInspeccion { get; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Direccion DireccionInspeccion { get; set; }
        public List<string> Bloques { get; set; }

        public Inspeccion(string nombre, DateTime fechaInicio, DateTime fechaFin, Direccion direccionInspeccion)
        {
            IdInspeccion = Guid.NewGuid();
            Nombre = nombre;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            DireccionInspeccion = direccionInspeccion;
            Bloques = new List<string>();
        }
    }
}
