using System;
using System.Collections.Generic;
namespace InspectionManager.Modelo
{
    public class Inspector
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public List<string> Inspecciones { get; set; }

        public Inspector(string dni, string nombre, string apellidos, string usuario, string password, DateTime fechaNacimiento)
        {
            Dni = dni;
            Nombre = nombre;
            Apellidos = apellidos;
            Usuario = usuario;
            Password = password;
            FechaNacimiento = fechaNacimiento;
            Inspecciones = new List<string>();
        }
    }
}
