using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Plantilla
    {
        public Guid IdPlantilla { get; }
        public string Nombre { get; set; }
        public TipoTrabajo Trabajo { get; set; }
        public string VersionActual { get; set; }
        public List<string> Versiones { get; set; }
        public List<Bloque> BloquesPlantilla { get; set; }

        public Plantilla(string nombre, TipoTrabajo tipoTrabajo)
        {
            IdPlantilla = Guid.NewGuid();
            Nombre = nombre;
            Trabajo = tipoTrabajo;
            VersionActual = "1.0";
            Versiones = new List<string>();
            BloquesPlantilla = new List<Bloque>();
        }

        public void ActualizarVersion(string nuevaVersion)
        {
            if (!Versiones.Contains(nuevaVersion)&&!VersionActual.Equals(nuevaVersion))
            {
                Versiones.Add(VersionActual);
                VersionActual = nuevaVersion;
            }
        }

        public void AddBloque(Bloque bloque)
        {
            BloquesPlantilla.Add(bloque);
        }
    }
}
