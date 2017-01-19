//using VisualGrading.Helpers;
//using VisualGrading.Students;
//using VisualGrading.Grades;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using VisualGrading.Tests;
//using Microsoft.Practices.Unity;
//using VisualGrading.DataAccess;
//using StudentTestReporting.Helpers;
//using VisualGrading.Business;

//namespace VisualGrading.Grades
//{
//    //TODO: Update Interface
//    public sealed class GradeRepository : AbstractRepository, INotifyPropertyChanged, IGradeRepository
//    {
//        #region Singleton Implementation

//        private static GradeRepository _instance = new GradeRepository();

//        //need static constructor to avoid dependency issues
//        static GradeRepository()
//        {

//        }

//        private GradeRepository()
//        {
//            //_testRepository = ContainerHelper.Container.Resolve<ITestRepository>();
//            //_studentRepository = ContainerHelper.Container.Resolve<IStudentRepository>();
//            //_dataManager = ContainerHelper.Container.Resolve<IDataManager>();
//            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
//            //_studentRepository.StudentAdded += AddGradesByStudentAsync;

//            InitializeGradeList();
//        }

//        public static GradeRepository Instance
//        {
//            get
//            {
//                return _instance;
//            }
//        }
//        #endregion



//        #region Properties
//        private IBusinessManager _businessManager;


//        private List<Grade> _GradeList;

//        public List<Grade> GradeList
//        {
//            get { return _GradeList; }
//            set
//            {
//                if (_GradeList != value)
//                {
//                    _GradeList = value;
//                    PropertyChanged(null, new PropertyChangedEventArgs("GradeList"));
//                }

//            }
//        }

//        #endregion

//        #region Methods

//        private void InitializeGradeList()
//        {
//            //TODO: This logic can be improved
//            if (!File.Exists(Setting.GetFileLocationByType<List<Grade>>()))
//            {
//                //GenerateAndSaveGrades();
//            }

//            //List<Grade> tempGradeList = (List<Grade>) _dataManager.Load<Grade>();

//            // //file can be created but grades are empty...need to regenerate in such a case
//            // if (tempGradeList == null || tempGradeList.Count == 0)
//            //     //GenerateAndSaveGrades();

//            // //tempGradeList = _dataManager.Load<List<GradeDTO>>();

//            // GradeList = tempGradeList;
//        }

//        //TODO: All these objects should return in-memory grades first, if they exist. Otherwise they can load from file. 
//        public async Task<List<Grade>> GetGradesAsync()
//        {
//            //TODO: This whole method needs a rewrite
//            if (GradeList != null && GradeList.Count > 0)
//            {
//                return GradeList;
//            }

//            List<Grade> grades = new List<Grade>();

//            //TODO: Make this file location dependent on a setting
//            //TODO: GradeDTO when the file and/or folder don't exist - causes issues

//            try
//            {
//                if (!File.Exists(Setting.GetFileLocationByType<List<Grade>>()))
//                {
//                    await _dataManager.SaveAsync<List<Grade>>(grades);
//                }

//                grades = await _dataManager.LoadAsync<List<Grade>>();
//            }
//            catch
//            {
//            }

//            //TODO: Come up with a better design for keeping GradeRepository in line with wherever else needs this information
//            if (GradeList == null)
//            {
//                GradeList = grades;
//            }

//            return grades;
//        }


//        public async void UpdateGradeAsync(Grade)
//        {
//            await _businessManager.SaveGrade(grade);
//        }

//        public async void AddGradesByTestAsync(Test test)
//        {

//        }

//        public async void AddGradesByStudentAsync(Student student)
//        {
//            foreach (var test in _testRepository.TestList)
//            {
//                GradeList.Add(new Grade(student, test));
//            }

//            await _dataManager.SaveAsync<List<Grade>>(GradeList);
//        }

//        public async void AddGradeAsync(Grade Grade)
//        {
//            GradeList.Add(Grade);
//            await _dataManager.SaveAsync<List<Grade>>(GradeList);
//        }

//        public async void RemoveGradesByStudentAsync(Student studentToRemove)
//        {

//            await _dataManager.SaveAsync<List<Grade>>(GradeList);
//        }

//        public async void RemoveGradesByTestAsync(Test testToRemove)
//        {
//            await _dataManager.SaveAsync<List<Grade>>(GradeList);
//        }

//        public event PropertyChangedEventHandler PropertyChanged = delegate { };

//        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        #endregion


//    }
//}

