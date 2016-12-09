using System.Threading.Tasks;

namespace StudentTestReporting.DataAccess
{
    public interface IDataManager
    {
        T Load<T>();
        Task<T> LoadAsync<T>();
        void Save<T>(object objectToSave);
        Task SaveAsync<T>(object objectToSave);
    }
}