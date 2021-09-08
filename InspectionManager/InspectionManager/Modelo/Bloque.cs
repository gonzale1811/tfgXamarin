using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Bloque
    {
        public Guid IdBloque { get; set; }
        public string Nombre { get; set; }
        public string PuestoTrabajo { get; set; }
        public List<string> PreguntasTexto { get; set; }
        public List<string> PreguntasBoolean { get; set; }
        public List<string> PreguntasValor { get; set; }
        public List<string> Fotografias { get; set; }

        public Bloque(string nombre, string puestoTrabajo)
        {
            IdBloque = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = puestoTrabajo;
            PreguntasTexto = new List<string>();
            PreguntasBoolean = new List<string>();
            PreguntasValor = new List<string>();
        }

        public Bloque(string nombre)
        {
            IdBloque = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = "";
            PreguntasTexto = new List<string>();
            PreguntasBoolean = new List<string>();
            PreguntasValor = new List<string>();
        }

        public void AddPreguntaTexto(string pregunta)
        {
            PreguntasTexto.Add(pregunta);
        }

        public void AddPreguntaBoolean(string pregunta)
        {
            PreguntasBoolean.Add(pregunta);
        }

        public void AddPreguntaValor(string pregunta)
        {
            PreguntasValor.Add(pregunta);
        }
    }
}
