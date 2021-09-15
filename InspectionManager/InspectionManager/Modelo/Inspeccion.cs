using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Inspeccion
    {
        public Guid IdInspeccion { get; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public Direccion DireccionInspeccion { get; set; }
        public List<string> Bloques { get; set; }

        public Inspeccion(string nombre, string fechaInicio, string fechaFin, Direccion direccionInspeccion)
        {
            IdInspeccion = Guid.NewGuid();
            Nombre = nombre;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            DireccionInspeccion = direccionInspeccion;
            Bloques = new List<string>();
        }

        public Inspeccion(Guid idInspeccion, string nombre, string fechaInicio, string fechaFin)
        {
            IdInspeccion = idInspeccion;
            Nombre = nombre;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            DireccionInspeccion = new Direccion("","","","");
            Bloques = new List<string>();
        }
    }
}
