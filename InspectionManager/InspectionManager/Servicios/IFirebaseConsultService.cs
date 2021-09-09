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

        string UploadFoto(string idInspeccion, string idBloque, int foto, Stream image);

        void AddBloqueInspeccion(Bloque bloque);

        void AddPreguntasTexto(List<IPregunta<string>> preguntasTexto);

        void AddPreguntasBoolean(List<IPregunta<bool>> preguntasBoolean);

        void AddPreguntasValor(List<IPregunta<int>> preguntasValor);

        void SetBloquesToInspeccion(Inspeccion inspeccion);

        void CancelarCreacionInspeccion(Inspeccion inspeccion, List<Bloque> bloques);
    }
}
