using System;
using System.Collections.Generic;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public interface IFirebaseConsultService
    {
        void AddInspector(Inspector inspector);

        Inspector GetInspectorByEmail(string email);
    }
}
