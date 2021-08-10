using System;
namespace InspectionManager.Modelo
{
    public class PreguntaBoolean: IPregunta<bool>
    {
        public PreguntaBoolean(string nombre, string tipoTrabajo)
        {
            IdPregunta = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = tipoTrabajo;
            RespuestaPregunta = null;
        }

        public Guid IdPregunta { get; }
        public string Nombre { get; set; }
        public string PuestoTrabajo { get; set; }
        public IRespuesta<bool> RespuestaPregunta { get; set; }

        public void Responder(bool valor)
        {
            IRespuesta<bool> respuesta = new RespuestaBoolean(valor);
            RespuestaPregunta = respuesta;
        }

        public void EditarRespuesta(bool valor)
        {
            RespuestaPregunta.ValorRespuesta = valor;
        }
    }
}
