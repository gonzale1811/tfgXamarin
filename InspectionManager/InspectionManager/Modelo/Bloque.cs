using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Bloque
    {
        public Guid IdBloque { get; }
        public string Nombre { get; set; }
        public string PuestoTrabajo { get; set; }
        public List<Pregunta> Preguntas { get; set; }

        public Bloque(string nombre, string puestoTrabajo)
        {
            IdBloque = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = puestoTrabajo;
            Preguntas = new List<Pregunta>();
        }
    }
}
