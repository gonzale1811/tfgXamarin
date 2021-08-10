using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace InspectionManager.Modelo
{
    public class RespuestaTexto: IRespuesta<string>
    {
        public RespuestaTexto(string valor)
        {
            ValorRespuesta = valor;
            Imagenes = new List<Image>();
        }

        public string ValorRespuesta { get; set; }

        public List<Image> Imagenes { get; set; }

        public void AddImage(Image imagen)
        {

        }
    }
}
