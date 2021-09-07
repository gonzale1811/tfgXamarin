using System;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public interface IFirebaseConsultService
    {
        void AddInspector(Inspector inspector);

        Inspector GetInspectorByEmail(string email);

        void AddInspeccionToInspector(Inspector inspector, Inspeccion inspeccion);

        void AddInspeccion(Inspeccion inspeccion);
    }
}
