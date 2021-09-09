using System;
namespace InspectionManager.Modelo
{
    public class PreguntaTexto : IPregunta<string>
    {

        public PreguntaTexto(string nombre)
        {
            IdPregunta = Guid.NewGuid();
            Nombre = nombre;
            RespuestaPregunta = null;
        }

        public PreguntaTexto(Guid idPregunta, string nombre)
        {
            IdPregunta = idPregunta;
            Nombre = nombre;
            RespuestaPregunta = null;
        }

        public Guid IdPregunta { get; set; }
        public string Nombre { get; set; }
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
