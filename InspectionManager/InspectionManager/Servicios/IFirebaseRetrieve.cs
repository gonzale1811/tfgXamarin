using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InspectionManager.Servicios
{
    public interface IFirebaseRetrieve<T>
    {
        T GetOne(string id);
        Task<T> GetOneAsync(string id);

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
