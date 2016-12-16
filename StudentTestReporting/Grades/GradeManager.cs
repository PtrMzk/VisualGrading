using VisualGrading.Helpers;
using VisualGrading.Students;
using VisualGrading.Grades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Tests;
using Microsoft.Practices.Unity;
using StudentTestReporting.DataAccess;
using StudentTestReporting.Helpers;

namespace VisualGrading.Grades
{
    //TODO: Update Interface
    public sealed class GradeManager : AbstractManager, INotifyPropertyChanged, IGradeManager
    {
        #region Singleton Implementation

        private static GradeManager _instance = new GradeManager();

        //need static constructor to avoid dependency issues
        static GradeManager()
        {
            
        }

        private GradeManager()
        {
            //_testManager = ContainerHelper.Container.Resolve<ITestManager>();
            //_studentManager = ContainerHelper.Container.Resolve<IStudentManager>();
            _dataManager= ContainerHelper.Container.Resolve<IDataManager>();
            //_studentManager.StudentAdded += AddGradesByStudentAsync;

            InitializeGradeList();
        }

        public static GradeManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion



        #region Properties
        private ITestManager _testManager;
        private IStudentManager _studentManager;
        private IDataManager _dataManager;
        
        private List<Grade> _GradeList;

        public List<Grade> GradeList
        {
            get { return _GradeList; }
            set
            {
                if (_GradeList != value)
                {
                    _GradeList = value;
                    PropertyChanged(null, new PropertyChangedEventArgs("GradeList"));
                }

            }
        }

        #endregion

        #region Methods

        private void InitializeGradeList()
        {
            //TODO: This logic can be improved
            if (!File.Exists(settingManager.GetFileLocationByType<List<Grade>>()))
            {
                GenerateAndSaveGrades();
            }

            List<Grade> tempGradeList = _dataManager.Load<List<Grade>>();

            //file can be created but grades are empty...need to regenerate in such a case
            if (tempGradeList == null || tempGradeList.Count == 0)
                GenerateAndSaveGrades();

            tempGradeList = _dataManager.Load<List<Grade>>();

            GradeList = tempGradeList;
        }

        //TODO: All these objects should return in-memory grades first, if they exist. Otherwise they can load from file. 
        public async Task<List<Grade>> GetGradesAsync()
        {
            //TODO: This whole method needs a rewrite
            if (GradeList != null && GradeList.Count > 0)
            {
                return GradeList;
            }

            List<Grade> grades = new List<Grade>();

            //TODO: Make this file location dependent on a setting
            //TODO: Grade when the file and/or folder don't exist - causes issues

            try
            {
                if (!File.Exists(settingManager.GetFileLocationByType<List<Grade>>()))
                {
                    await _dataManager.SaveAsync<List<Grade>>(grades);
                }

                grades = await _dataManager.LoadAsync<List<Grade>>();
            }
            catch
            {
            }

            //TODO: Come up with a better design for keeping GradeManager in line with wherever else needs this information
            if (GradeList == null)
            {
                GradeList = grades;
            }

            return grades;
        }


        public async void UpdateGradeAsync(Grade updatedGrade)
        {
            GradeList.Select(g => g = updatedGrade).Where(g => g.GradeID == updatedGrade.GradeID);

            await _dataManager.SaveAsync<List<Grade>>(GradeList);
        }

        public async void AddGradesByTestAsync(Test test)
        {

        }

        public async void AddGradesByStudentAsync(Student student)
        {
            foreach (var test in _testManager.TestList)
            {
                GradeList.Add(new Grade(student, test));
            }

            await _dataManager.SaveAsync<List<Grade>>(GradeList);
        }

        public async void AddGradeAsync(Grade Grade)
        {
            GradeList.Add(Grade);
            await _dataManager.SaveAsync<List<Grade>>(GradeList);
        }

        public async void RemoveGradesByStudentAsync(Student studentToRemove)
        {

            await _dataManager.SaveAsync<List<Grade>>(GradeList);
        }

        public async void RemoveGradesByTestAsync(Test testToRemove)
        {
            await _dataManager.SaveAsync<List<Grade>>(GradeList);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }
}

