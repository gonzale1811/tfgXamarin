using System;
using System.Collections.Generic;

namespace InspectionManager.Modelo
{
    public class Plantilla
    {
        public Guid IdPlantilla { get; set; }
        public string Nombre { get; set; }
        public TipoTrabajo Trabajo { get; set; }
        public string VersionActual { get; set; }
        public List<string> Versiones { get; set; }
        public List<string> BloquesPlantilla { get; set; }

        public Plantilla(string nombre, TipoTrabajo tipoTrabajo)
        {
            IdPlantilla = Guid.NewGuid();
            Nombre = nombre;
            Trabajo = tipoTrabajo;
            VersionActual = "1.0";
            Versiones = new List<string>();
            BloquesPlantilla = new List<string>();
        }

        public Plantilla(string nombre, TipoTrabajo tipoTrabajo, string version)
        {
            IdPlantilla = Guid.NewGuid();
            Nombre = nombre;
            Trabajo = tipoTrabajo;
            VersionActual = version;
            Versiones = new List<string>();
            BloquesPlantilla = new List<string>();
        }

        public void ActualizarVersion(string nuevaVersion)
        {
            if (!Versiones.Contains(nuevaVersion)&&!VersionActual.Equals(nuevaVersion))
            {
                Versiones.Add(VersionActual);
                VersionActual = nuevaVersion;
            }
        }

        public void AddBloque(string bloque)
        {
            BloquesPlantilla.Add(bloque);
        }

        public string TrabajoToString()
        {
            switch (Trabajo)
            {
                case TipoTrabajo.Obra:
                    return "Obra";
                case TipoTrabajo.Oficina:
                    return "Oficina";
                case TipoTrabajo.Fabrica:
                    return "Fabrica";
                case TipoTrabajo.Servicios:
                    return "Servicios";
                default:
                    return null;
            }
        }

        public List<string> PuestosDelTipoTrabajo()
        {
            switch (Trabajo)
            {
                case TipoTrabajo.Oficina:
                    return new List<string>() { "Secretario/a", "Oficinista", "Jefe de proyecto", "Jefe de equipo" };
                case TipoTrabajo.Obra:
                    return new List<string>() { "Peon", "Oficial", "Encargado", "Jefe de obra" };
                case TipoTrabajo.Fabrica:
                    return new List<string>() { "Peon", "Maquinista", "Encargado" };
                case TipoTrabajo.Servicios:
                    return new List<string>() { "Hosteleria", "Turismo", "Medico/a", "Profesor/a" };
                default:
                    return new List<string>();
            }
        }
    }
}
