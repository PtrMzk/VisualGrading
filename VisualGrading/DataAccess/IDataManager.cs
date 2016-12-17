using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VisualGrading.DataAccess
{
    public interface IDataManager
    {
        List<T> Load<T>();
        Task<T> LoadAsync<T>();
        void Save<T>(object objectToSave);
        Task SaveAsync<T>(object objectToSave);
    }
}