using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace InspectionManager.Modelo
{
    public interface IRespuesta<T>
    {
        T ValorRespuesta { get; set; }

        List<Image> Imagenes { get; set; }

        void AddImage(Image imagen);
    }
}
