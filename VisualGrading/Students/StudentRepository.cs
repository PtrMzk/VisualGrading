using VisualGrading.Grades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading;
using VisualGrading.DataAccess;
using Microsoft.Practices.Unity;
using StudentTestReporting.Helpers;

namespace VisualGrading.Students
{
    public sealed class StudentRepository : AbstractRepository, INotifyPropertyChanged, IStudentRepository
    {
        #region Singleton Implementation

        private static StudentRepository _instance = new StudentRepository();

        private StudentRepository()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            //_gradeRepository = ContainerHelper.Container.Resolve<IGradeRepository>();
            InitializeStudentList();
        }

        //need static constructor to avoid dependency issues
        static StudentRepository()
        {
            
        }

        public static StudentRepository Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties

        private IDataManager _dataManager;
        private IGradeRepository _gradeRepository;

        private List<Student> _StudentList;

        public List<Student> StudentList
        {
            get
            {
                return _StudentList;
            }
            set
            {
                if (_StudentList != value)
                {
                    _StudentList = value;
                    PropertyChanged(null, new PropertyChangedEventArgs("StudentList"));
                }
            }
        }

        //public string StudentFileLocation { get { return settingRepository.StudentFileLocation; } }
        #endregion

        #region Methods

        private void InitializeStudentList()
        {
            if (!File.Exists(settingRepository.GetFileLocationByType<List<Student>>()))
            {
                List<Student> emptyStudentList = new List<Student>();
                _dataManager.Save<List<Student>>(emptyStudentList);
            }
            StudentList = _dataManager.Load<Student>();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            //TODO: This whole method needs a rewrite
            if (StudentList != null && StudentList.Count > 0)
            {
                return StudentList;
            }

            List<Student> Students = new List<Student>();

            //TODO: Make this file location dependent on a setting
            //TODO: Student when the file and/or folder don't exist - causes issues

            //TODO: just test students for now...
            if (Students == null || Students.Count == 0)
            {
                Students = new List<Student>();
                //ObservableStudents = new List<Student>();
                Student jack = new Student() { FirstName = "Jack", LastName = "Dawson", EmailAddress = "PokerMaster@gmail.com", OverallGrade = 95.7m };
                Student rose = new Student() { FirstName = "Rose DeWitt", LastName = "Buktater", Nickname = "Rose", EmailAddress = "IllNeverLetGo@hotmail.com", ParentEmailAddress = "CalFanz@yahoo.com", OverallGrade = 93.4m };
                Student cal = new Student() { FirstName = "Cal", LastName = "Hockley", EmailAddress = "LeftHeartOfTheOceanInMyJacketPocket@hotmail.com", OverallGrade = 78.3m };
                //Student = BinarySerialization.ReadFromBinaryFile<Student>(@"C:\Visual Studio Code\VisualGrading\VisualGrading\SaveFiles\Student.json");
                Students.Add(jack);
                Students.Add(rose);
                Students.Add(cal);
            }
            else
                Students = await _dataManager.LoadAsync<List<Student>>();
            

            //TODO: Come up with a better design for keeping StudentRepository in line with wherever else needs this information
            if (StudentList == null)
            {
                StudentList = Students;
            }

            return Students;
        }

        public async void UpdateStudentAsync(Student updatedStudent)
        {
            foreach (Student cachedStudent in StudentList)
            {
                if (cachedStudent.ID == updatedStudent.ID)
                {
                    StudentList.Remove(cachedStudent);
                    StudentList.Add(updatedStudent);
                    break;
                }
            }
            await _dataManager.SaveAsync<List<Student>>(StudentList);
        }

        public async void AddStudentAsync(Student student)
        {
            StudentList.Add(student);

            if (StudentAdded != null)
                StudentAdded(student);

            _gradeRepository.AddGradesByStudentAsync(student);

            await _dataManager.SaveAsync<List<Student>>(StudentList);
        }

        public async void RemoveStudent(Student student)
        {
            foreach (Student cachedStudent in StudentList)
            {
                if (cachedStudent.ID == student.ID)
                {
                    StudentList.Remove(cachedStudent);
                    break;
                }
            }

            //_gradeRepository.RemoveGradesByStudentAsync(student);

            await _dataManager.SaveAsync<List<Student>>(StudentList);
        }

        public Student GetStudentByID(int studentID)
        {
            foreach (var cachedStudent in StudentList)
            {
                if (cachedStudent.ID == studentID)
                    return cachedStudent;
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Delegates
        public delegate void OnStudentChangedEventHandler(Student source);
        #endregion

        #region Events

        public event Action OnStudentUpdate = delegate { };

        public event Action OnStudentDelete = delegate { };

        public event OnStudentChangedEventHandler StudentAdded;

        #endregion
    }
}
