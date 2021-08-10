using System;
namespace InspectionManager.Modelo
{
    public class Direccion
    {
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }

        public Direccion(string calle, string numero, string localidad, string codigoPostal)
        {
            Calle = calle;
            Numero = numero;
            Localidad = localidad;
            CodigoPostal = codigoPostal;
        }
    }
}
