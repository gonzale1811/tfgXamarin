using System;
using System.Threading.Tasks;
using InspectionManager.Droid.Servicios;
using InspectionManager.Modelo;
using InspectionManager.Servicios;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseConsultService))]
namespace InspectionManager.Droid.Servicios
{
    public class FirebaseConsultService: IFirebaseConsultService
    {
        public FirebaseConsultService()
        {
        }

        public Task<bool> AddInspector(Inspector inspector)
        {
            throw new NotImplementedException();
        }
    }
}
