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

namespace VisualGrading.Grades
{
    //TODO: Update Interface
    public sealed class GradeManager : INotifyPropertyChanged, IGradeManager
    {
        #region Singleton Implementation

        private static GradeManager _instance = new GradeManager();

        static GradeManager()
        {
            Instance._testManager = ContainerHelper.Container.Resolve<ITestManager>();
            Instance._studentManager = ContainerHelper.Container.Resolve<IStudentManager>();
            Instance._studentManager.StudentAdded += Instance.AddGradeByStudentAsync;

            Instance.InitializeGradeList();
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

        public string GradeFileLocation { get { return SettingManager.Instance.GradeFileLocation;} }

        private List<Grade> _GradeList;

        public List<Grade> GradeList
        {
            get { return Instance._GradeList; }
            set
            {
                if (Instance._GradeList != value)
                {
                    Instance._GradeList = value;
                    Instance.PropertyChanged(null, new PropertyChangedEventArgs("GradeList"));
                }

            }
        }

        #endregion

        #region Methods

        private void InitializeGradeList()
        {
            if (!File.Exists(GradeFileLocation))
            {
                var grades = GenerateGrades(_studentManager.StudentList, _testManager.TestList);

                Helpers.JSONSerialization.SerializeJSON(GradeFileLocation, grades);
            }

            List<Grade> tempGradeList = JSONSerialization.DeserializeJSON<List<Grade>>(GradeFileLocation);

            //TODO: See if this is the correct way to do this
            //replace every test and student with a reference
            foreach (var grade in tempGradeList)
            {
                var testID = grade.Test.TestID;
                var studentID = grade.Student.StudentID;

                grade.Test = _testManager.GetTestByID(testID);
                grade.Student = _studentManager.GetStudentByID(studentID);

            }

            GradeList = tempGradeList;
        }

        public async Task<List<Grade>> GetGradesAsync()
        {
            string GradeFileLocation = SettingManager.Instance.GradeFileLocation;

            //TODO: This whole method needs a rewrite
            if (GradeList != null && GradeList.Count > 0)
            {
                return GradeList;
            }

            List<Grade> Grades = new List<Grade>();

            //TODO: Make this file location dependent on a setting
            //TODO: Grade when the file and/or folder don't exist - causes issues

            try
            {
                if (!File.Exists(GradeFileLocation))
                {
                    await Helpers.JSONSerialization.SerializeJSONAsync(GradeFileLocation, Grades);
                }

                Grades = await Helpers.JSONSerialization.DeserializeJSONAsync<List<Grade>>(GradeFileLocation);
            }
            catch
            {
            }

            //TODO: Come up with a better design for keeping GradeManager in line with wherever else needs this information
            if (GradeList == null)
            {
                GradeList = Grades;
            }

            return Grades;
        }


        public async void UpdateGradeAsync(Grade updatedGrade)
        {
            foreach (Grade currentGrade in GradeList)
            {

            }
            await JSONSerialization.SerializeJSONAsync(
                SettingManager.Instance.GradeFileLocation, GradeList);
        }

        public async void AddGradeByTestAsync(Test test)
        {

        }

        public async void AddGradeByStudentAsync(Student student)
        {
            foreach (var test in _testManager.TestList)
            {
                GradeList.Add(new Grade(student, test));
            }

            await JSONSerialization.SerializeJSONAsync(
    SettingManager.Instance.GradeFileLocation, GradeList);
        }

        public async void AddGradeAsync(Grade Grade)
        {
            GradeList.Add(Grade);
            await JSONSerialization.SerializeJSONAsync(
                SettingManager.Instance.GradeFileLocation, GradeList);
        }

        public async void RemoveGradeByStudentAsync(Student studentToRemove)
        {

            await JSONSerialization.SerializeJSONAsync(
                SettingManager.Instance.GradeFileLocation, GradeList);
        }

        public async void RemoveGradeByTestAsync(Test testToRemove)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public List<Grade> GenerateGrades(List<Student> students, List<Test> tests )
        {
            {
                var grades = (from student in students
                                from test in tests
                                 select new Grade
                                 (
                                    student, test
                                 )
                                 
                                    
                                 ).ToList();

                return grades;
            }
        }
    }
}

