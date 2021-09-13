using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InspectionManager.Modelo;
using Xamarin.Forms;

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

        void GenerarPdfInspeccion(Inspeccion inspeccion, List<Bloque> bloques);

        List<Bloque> GetBloquesByInspeccion(Inspeccion inspeccion);

        List<IPregunta<string>> GetPreguntasTextoByBloqueInspeccion(Bloque bloque);

        List<IPregunta<bool>> GetPreguntasBooleanByBloqueInspeccion(Bloque bloque);

        List<IPregunta<int>> GetPreguntasValorByBloqueInspeccion(Bloque bloque);

        void DeleteInspeccion(Inspector inspector, Inspeccion inspeccion, List<Bloque> bloques);

        void ActualizarInspeccion(Inspeccion inspeccion);

        Task<string> DonwloadImage(string idInspeccion, string idBloque, string nombreImagen);

        void ActualizarPreguntasTextoInspeccion(List<IPregunta<string>> preguntasTexto);

        void ActualizarPreguntasBooleanInspeccion(List<IPregunta<bool>> preguntasBoolean);

        void ActualizarPreguntasValorInspeccion(List<IPregunta<int>> preguntasValor);

        void ActualizarInformacionUsuario(Inspector inspector);

        void UpdateFotografiasBloque(Bloque bloque);
    }
}
