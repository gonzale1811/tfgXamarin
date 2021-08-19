using System;
using System.Threading.Tasks;
using InspectionManager.Modelo;

namespace InspectionManager.Servicios
{
    public interface IFirebaseConsultService
    {
        Task<bool> AddInspector(Inspector inspector);
    }
}
