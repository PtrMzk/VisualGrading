using StudentTestReporting.Helpers;
using StudentTestReporting.Students;
using StudentTestReporting.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.Grades
{
    public sealed class GradeManager
    {
        #region Singleton Implementation
        static readonly GradeManager instance = new GradeManager();

        static GradeManager()
        {
            try
            {
                grades = BinarySerialization.ReadFromBinaryFile<List<Grade>>(@"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\grade.bin");
            }
            catch
            {
                grades = new List<Grade>();
            }
        }

        public static GradeManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion  

        public static List<Grade> grades { get; set; }
       
        public static void GenerateGrades(List<Test> tests)
        {
            GenerateGrades(StudentManager.students, tests);
        }

        public static void GenerateGrades(List<Student> students)
        {
            //TODO: Make this not use the TestManager instance
            //GenerateGrades(students, TestManager.ObservableTests);
        }
           

        public static void GenerateGrades(List<Student> students, List<Test> tests)
        {
            {
                var crossJoin = (from a in students
                                 from b in tests
                                     //from c in grades
                                 select new
                                 {
                                     Name = a.Name,
                                     Nickname = a.Nickname,
                                     Subject = b.Subject,
                                     TestNumber = b.TestNumber,
                                     Grades = 0
                                 }).ToList();

                //Anonymous types cannot be edited in datagridview. Need to add results to a List<Grade> so we can edit the grades. 
                foreach (var item in crossJoin)
                {
                    //first check if the entry already exists
                    int inList = -1;

                    inList = grades.FindIndex(g => g.Name + "||" + g.Subject + "||" + g.TestNumber == item.Name + "||" + item.Subject + "||" + item.TestNumber);

                    if (inList == -1)
                    {
                        grades.Add(new Grade()
                        {
                            Name = item.Name
                            ,
                            Nickname = item.Nickname
                            ,
                            Subject = item.Subject
                            ,
                            TestNumber = item.TestNumber
                            ,
                            Grades = item.Grades
                        }
                        );
                    }
                }
            }
        }





    }
}
