using System;
using System.Collections.Generic;
using System.IO;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public interface IFirebaseConsultService
    {
        void AddInspector(Inspector inspector);

        Inspector GetInspectorByEmail(string email);

        void AddInspeccionToInspector(Inspector inspector, Inspeccion inspeccion);

        void AddInspeccion(Inspeccion inspeccion);

        List<Plantilla> GetPlantillas();

        List<Bloque> GetBloquesByPlantilla(Plantilla plantilla);

        List<IPregunta<string>> GetPreguntasTextoByBloque(Bloque bloque);

        List<IPregunta<bool>> GetPreguntasBooleanByBloque(Bloque bloque);

        List<IPregunta<int>> GetPreguntasValorByBloque(Bloque bloque);

        string UploadFoto(Stream image);
    }
}
