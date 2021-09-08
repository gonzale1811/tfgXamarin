using System;
namespace InspectionManager.Modelo
{
    public class PreguntaTexto : IPregunta<string>
    {

        public PreguntaTexto(string nombre, string tipoTrabajo)
        {
            IdPregunta = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = tipoTrabajo;
            RespuestaPregunta = null;
        }

        public PreguntaTexto(string nombre)
        {
            IdPregunta = Guid.NewGuid();
            Nombre = nombre;
            PuestoTrabajo = null;
            RespuestaPregunta = null;
        }

        public Guid IdPregunta { get; set; }
        public string Nombre { get; set; }
        public string PuestoTrabajo { get; set; }
        public IRespuesta<string> RespuestaPregunta { get; set; }

        public void Responder(string texto)
        {
            IRespuesta<string> respuesta = new RespuestaTexto(texto);
            RespuestaPregunta = respuesta;
        }

        public void EditarRespuesta(string valor)
        {
            RespuestaPregunta.ValorRespuesta = valor;
        }
    }
}
