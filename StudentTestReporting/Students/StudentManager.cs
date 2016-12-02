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
using VisualGrading.Helpers;
using VisualGrading;

namespace VisualGrading.Students
{
    public sealed class StudentManager : INotifyPropertyChanged, IStudentManager
    {

        #region Singleton Implementation

        private static StudentManager _instance = new StudentManager();

        static StudentManager()
        {
        }

        public static StudentManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties

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
                    Instance.PropertyChanged(null, new PropertyChangedEventArgs("StudentList"));
                }

            }
        }
        #endregion

        #region Methods
        public async Task<List<Student>> GetStudentsAsync()
        {
            string StudentFileLocation = SettingManager.Instance.StudentFileLocation;

            //TODO: This whole method needs a rewrite
            if (StudentList != null && StudentList.Count > 0)
            {
                return StudentList;
            }

            List<Student> Students = new List<Student>();

            //TODO: Make this file location dependent on a setting
            //TODO: Student when the file and/or folder don't exist - causes issues

            if (Students == null || Students.Count == 0)
            {
                Students = new List<Student>();
                //ObservableStudents = new List<Student>();
                Student jack = new Student() { FirstName = "Jack", LastName = "Dawson",  EmailAddress = "PokerMaster@gmail.com", OverallGrade = 95.7m};
                Student rose = new Student() { FirstName = "Rose DeWitt", LastName = "Buktater", Nickname = "Rose", EmailAddress = "IllNeverLetGo@hotmail.com", ParentEmailAddress = "CalFanz@yahoo.com", OverallGrade = 93.4m};
                Student cal = new Student() { FirstName = "Cal", LastName = "Hockley",  EmailAddress = "LeftHeartOfTheOceanInMyJacketPocket@hotmail.com", OverallGrade = 78.3m};
                //Student = BinarySerialization.ReadFromBinaryFile<Student>(@"C:\Visual Studio Code\VisualGrading\VisualGrading\SaveFiles\Student.json");
                Students.Add(jack);
                Students.Add(rose);
                Students.Add(cal);
               }

            try
            {
                if (!File.Exists(StudentFileLocation))
                {
                    await Helpers.JSONSerialization.SerializeJSONAsync(StudentFileLocation, Students);
                }

                Students = await Helpers.JSONSerialization.DeserializeJSONAsync<List<Student>>(StudentFileLocation);
            }
            catch
            {
            }

            //TODO: Come up with a better design for keeping StudentManager in line with wherever else needs this information
            if (StudentList == null)
            {
                StudentList = Students;
            }

            return Students;
        }

        public async void UpdateStudentAsync(Student updatedStudent)
        {
            foreach (Student currentStudent in StudentList)
            {
                if (currentStudent.StudentID == updatedStudent.StudentID)
                {
                    StudentList.Remove(currentStudent);
                    StudentList.Add(updatedStudent);
                    break;
                }
            }
            GradeManager.GenerateGrades(StudentList);
            await JSONSerialization.SerializeJSONAsync(
            SettingManager.Instance.StudentFileLocation, StudentList);
        }

        public async void AddStudentAsync(Student Student)
        {
            StudentList.Add(Student);
            GradeManager.GenerateGrades(StudentList);
            await JSONSerialization.SerializeJSONAsync(
                SettingManager.Instance.StudentFileLocation, StudentList);
        }

       public async void RemoveStudent(Student StudentToDelete)
        {
            foreach (Student currentStudent in StudentList)
            {
                if (currentStudent.StudentID == StudentToDelete.StudentID)
                {
                    StudentList.Remove(currentStudent);
                    break;
                }
            }
            GradeManager.GenerateGrades(StudentList);
            await JSONSerialization.SerializeJSONAsync(
            SettingManager.Instance.StudentFileLocation, StudentList);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
