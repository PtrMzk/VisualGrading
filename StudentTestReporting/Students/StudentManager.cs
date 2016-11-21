using StudentTestReporting.Grades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting;

namespace StudentTestReporting.Students
{
    public sealed class StudentManager
    {
        #region Singleton Implementation
        static readonly StudentManager instance = new StudentManager();

        static StudentManager()
        {
            students = new List<Student>();
            try
            {
                //TODO change this to read a list of students
                
                Student student = new Student() { Name = "Piotr", Nickname = "Big P" };
                //student = BinarySerialization.ReadFromBinaryFile<Student>(@"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\student.bin");
                students.Add(student);
                //TODO: Make not use TestManager 
                //GradeManager.GenerateGrades(StudentManager.students, TestManager.tests);
            }
            catch
            {

            }
        }

        public static StudentManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public static List<Student> students { get; set; }

        public void Add(Student student)
        {
            students.Add(student);
            GradeManager.GenerateGrades(students);
        }

        public void Remove(Student student)
        {

        }

    }
}
