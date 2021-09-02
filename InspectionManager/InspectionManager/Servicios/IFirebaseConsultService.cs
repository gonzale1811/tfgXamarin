using System;
using System.Threading.Tasks;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public interface IFirebaseConsultService
    {
        void AddInspector(Inspector inspector);
    }
}
