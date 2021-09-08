using System;
namespace InspectionManager.Modelo
{
    public class PreguntaValor: IPregunta<int>
    {
        public PreguntaValor(string nombre, string puestoTrabajo)
        {
            IdPregunta = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = puestoTrabajo;
            RespuestaPregunta = null;
        }

        public PreguntaValor(Guid idPregunta, string nombre)
        {
            IdPregunta = idPregunta;
            Nombre = nombre;
            PuestoTrabajo = null;
            RespuestaPregunta = null;
        }

        public Guid IdPregunta { get; set; }
        public string Nombre { get; set; }
        public string PuestoTrabajo { get; set; }
        public IRespuesta<int> RespuestaPregunta { get; set; }

        public void Responder(int valor)
        {
            IRespuesta<int> respuesta = new RespuestaValor(valor);
            RespuestaPregunta = respuesta;
        }

        public void EditarRespuesta(int valor)
        {
            RespuestaPregunta.ValorRespuesta = valor;
        }
    }
}
