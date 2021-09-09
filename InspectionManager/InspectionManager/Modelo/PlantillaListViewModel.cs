using System;
namespace InspectionManager.Modelo
{
    public class PlantillaListViewModel
    {
        public string Id { get; }
        public string Nombre { get; set; }
        public string Version { get; set; }
        public string TrabajoImagen { get; set; }

        public PlantillaListViewModel(string Id, string Nombre, string Version, TipoTrabajo TrabajoImagen)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Version = "Version de la plantilla: "+Version;
            this.TrabajoImagen = GetImagenTrabajo(TrabajoImagen);
        }

        private string GetImagenTrabajo(TipoTrabajo trabajo)
        {
            switch (trabajo)
            {
                case TipoTrabajo.Obra:
                    return "obra_black.png";
                case TipoTrabajo.Oficina:
                    return "oficina_black.png";
                case TipoTrabajo.Fabrica:
                    return "fabrica_black.png";
                case TipoTrabajo.Servicios:
                    return "servicios_black.png";
                default:
                    return "";
            }
        }
    }
}
