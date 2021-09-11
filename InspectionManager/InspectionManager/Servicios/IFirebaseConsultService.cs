﻿using System;
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

        void AddPreguntasTexto(List<IPregunta<string>> preguntasTexto, string coleccion, bool paraInspeccion);

        void AddPreguntasBoolean(List<IPregunta<bool>> preguntasBoolean, string coleccion, bool paraInspeccion);

        void AddPreguntasValor(List<IPregunta<int>> preguntasValor, string coleccion, bool paraInspeccion);

        void SetBloquesToInspeccion(Inspeccion inspeccion);

        void CancelarCreacionInspeccion(Inspeccion inspeccion, List<Bloque> bloques);

        void AddPlantilla(Plantilla plantilla);

        void AddBloquePlantilla(Bloque bloque);

        void SetBloquesToPlantilla(Plantilla plantilla);

        void CancelarCreacionPlantilla(Plantilla plantilla, List<Bloque> bloques);

        List<Inspeccion> GetInspeccionesByUsuario(Inspector inspector);

        void GenerarPdfInspeccion(Inspeccion inspeccion);

        List<Bloque> GetBloquesByInspeccion(Inspeccion inspeccion);

        List<IPregunta<string>> GetPreguntasTextoByBloqueInspeccion(Bloque bloque);

        List<IPregunta<bool>> GetPreguntasBooleanByBloqueInspeccion(Bloque bloque);

        List<IPregunta<int>> GetPreguntasValorByBloqueInspeccion(Bloque bloque);
    }
}
