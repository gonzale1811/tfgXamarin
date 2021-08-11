using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Bloque
    {
        public Guid IdBloque { get; }
        public string Nombre { get; set; }
        public string PuestoTrabajo { get; set; }
        public List<IPregunta<string>> PreguntasTexto { get; set; }
        public List<IPregunta<bool>> PreguntasBoolean { get; set; }
        public List<IPregunta<int>> PreguntasValor { get; set; }

        public Bloque(string nombre, string puestoTrabajo)
        {
            IdBloque = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = puestoTrabajo;
            PreguntasTexto = new List<IPregunta<string>>();
            PreguntasBoolean = new List<IPregunta<bool>>();
            PreguntasValor = new List<IPregunta<int>>();
        }

        public void AddPreguntaTexto(IPregunta<string> pregunta)
        {
            PreguntasTexto.Add(pregunta);
        }

        public void AddPreguntaBoolean(IPregunta<bool> pregunta)
        {
            PreguntasBoolean.Add(pregunta);
        }

        public void AddPreguntaValor(IPregunta<int> pregunta)
        {
            PreguntasValor.Add(pregunta);
        }
    }
}
