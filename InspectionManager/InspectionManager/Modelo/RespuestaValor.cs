using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace InspectionManager.Modelo
{
    public class RespuestaValor: IRespuesta<int>
    {
        public RespuestaValor(int valor)
        {
            ValorRespuesta = valor;
            Imagenes = new List<Image>();
        }

        public int ValorRespuesta { get; set; }
        public List<Image> Imagenes { get; set; }

        public void AddImage(Image imagen)
        {

        }
    }
}
