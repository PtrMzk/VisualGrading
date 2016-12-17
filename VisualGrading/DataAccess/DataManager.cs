using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Model.Data;
using VisualGrading.Model.Repositories;

namespace VisualGrading.DataAccess
{
    public class DataManager : IDataManager
    {
        #region Singleton Implementation

        private static DataManager _instance = new DataManager();

        private DataManager()
        {
            _unitOfWork = ContainerHelper.Container.Resolve<IUnitOfWork>();
            //var x = _unitOfWork.StudentRepository.FirstOrDefault();

            //var student = new VisualGrading.Model.Data.Student();
            //student.FirstName = "Wayne";
            //student.LastName = "Crosby";
            //_unitOfWork.StudentRepository.Add(student);

            //_unitOfWork.Commit();

            //var y = _unitOfWork.StudentRepository.GetAll();

            //var testStudent = new VisualGrading.Students.Student()
            //{
            //    FirstName = "Ted",
            //    LastName = "Gray",
            //    EmailAddress = "ted@ted.com"
            //};

            //var pocoStudent = Mapper.Map<VisualGrading.Model.Data.Student>(testStudent);

            //_unitOfWork.StudentRepository.Add(pocoStudent);
            //_unitOfWork.Commit();
            //var tempList = y.ToList();

            var a = FindCorrespondingTypeForAutoMapper<VisualGrading.Model.Data.Student>();
            var c = Type.GetType("VisualGrading.Model.Data.Student, VisualGrading.Model");

            var b = FindCorrespondingTypeForAutoMapper<VisualGrading.Students.Student>();

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

        private IUnitOfWork _unitOfWork;

        private SettingRepository _settingRepository => SettingRepository.Instance;
        #endregion

        #region Methods
        //private IList GenerateListByType(Type type)
        //{
        //    Type listType = typeof(List<>).MakeGenericType(type);
        //    return (IList)Activator.CreateInstance(listType);
        //}

        private Type GetCorrespondingTypeForAutoMapper<T>()
        {
            string typeNameForConversion;
            if (typeof(T).Namespace.StartsWith("VisualGrading.Model"))
            {
                typeNameForConversion = "VisualGrading." + typeof(T).Name + "s." + typeof(T).Name;
            }
            else
            {
                typeNameForConversion = "VisualGrading.Model.Data." + typeof(T).Name + ", VisualGrading.Model";
            }

            var type = Type.GetType(typeNameForConversion);

            return type;
        }

        private IRepository<> GetRepositoryForType<T>(IUnitOfWork _unitOfWork) where T : class, IEntity
        {
            switch (typeof(T).Name)
            {
                case "Student":
                    return (EFRepository<X>)_unitOfWork.StudentRepository;
                case "Test":
                    return (EFRepository<T>)_unitOfWork.TestRepository;
                case "Grade":
                    return (EFRepository<T>)_unitOfWork.GradeRepository;
            }
            return null;
        }

        public void Save<T>(object objectToSave)
        {

        }

        public async Task SaveAsync<T>(object objectToSave)
        {
            await JSONSerialization.SerializeJSONAsync(_settingRepository.GetFileLocationByType<T>(), objectToSave);
        }

        public List<T> Load<T>()
        {
            Type correspondingType = GetCorrespondingTypeForAutoMapper<T>();

            var repository = GetRepositoryForType<correspondingType.GetType()>

            var dbObjectsList = _unitOfWork..GetAll().ToList();

            var convertedList = new List<T>();

            foreach (var dbObject in dbObjectsList)
            {
                T convertedObject = (T)Mapper.Map(dbObject, dbObject.GetType(), typeof(T));
                convertedList.Add(convertedObject);
            }

            return convertedList;
        }

        public async Task<T> LoadAsync<T>()
        {
            return await JSONSerialization.DeserializeJSONAsync<T>(_settingRepository.GetFileLocationByType<T>());
        }

        //public void Save<T>(object objectToSave)
        //{
        //    JSONSerialization.SerializeJSON(_settingRepository.GetFileLocationByType<T>(), objectToSave);
        //}

        //public async Task SaveAsync<T>(object objectToSave)
        //{
        //    await JSONSerialization.SerializeJSONAsync(_settingRepository.GetFileLocationByType<T>(), objectToSave);
        //}

        //public T Load<T>()
        //{
        //    return JSONSerialization.DeserializeJSON<T>(_settingRepository.GetFileLocationByType<T>());
        //}

        //public async Task<T> LoadAsync<T>()
        //{
        //    return await JSONSerialization.DeserializeJSONAsync<T>(_settingRepository.GetFileLocationByType<T>());
        //}



        #endregion

    }
}
