using System;
namespace InspectionManager.Modelo
{
    public class PreguntaBoolean: IPregunta<bool>
    {
        public PreguntaBoolean(string nombre)
        {
            IdPregunta = Guid.NewGuid();
            Nombre = nombre;
            RespuestaPregunta = null;
        }

        public PreguntaBoolean(Guid idPregunta, string nombre)
        {
            IdPregunta = idPregunta;
            Nombre = nombre;
            RespuestaPregunta = null;
        }

        public Guid IdPregunta { get; set; }
        public string Nombre { get; set; }
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
