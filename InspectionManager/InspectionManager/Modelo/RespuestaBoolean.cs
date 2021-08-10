using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace InspectionManager.Modelo
{
    public class RespuestaBoolean: IRespuesta<bool>
    {
        public RespuestaBoolean(bool valor)
        {
            ValorRespuesta = valor;
            Imagenes = new List<Image>();
        }

        public bool ValorRespuesta { get; set; }
        public List<Image> Imagenes { get; set; }

        public void AddImage(Image imagen)
        {

        }
    }
}
