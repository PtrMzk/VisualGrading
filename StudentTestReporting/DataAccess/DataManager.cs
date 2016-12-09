using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;

namespace StudentTestReporting.DataAccess
{
    public class DataManager : IDataManager
    {
        #region Singleton Implementation

        private static DataManager _instance = new DataManager();

        private DataManager()
        {
        }

        public static DataManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties
        private SettingManager _settingManager => SettingManager.Instance;
        #endregion

        #region Methods
        public void Save<T>(object objectToSave)
        {
            JSONSerialization.SerializeJSON(_settingManager.GetFileLocationByType<T>(), objectToSave);
        }

        public async Task SaveAsync<T>(object objectToSave)
        {
            await JSONSerialization.SerializeJSONAsync(_settingManager.GetFileLocationByType<T>(), objectToSave);
        }

        public T Load<T>()
        {
            return JSONSerialization.DeserializeJSON<T>(_settingManager.GetFileLocationByType<T>());
        }

        public async Task<T> LoadAsync<T>()
        {
            return await JSONSerialization.DeserializeJSONAsync<T>(_settingManager.GetFileLocationByType<T>());
        }
        #endregion

    }
}
