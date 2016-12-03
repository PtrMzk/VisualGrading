using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Grades
{
    [Serializable]
    public class Grade
    {
        #region Constructors
        public Grade()
        {
            this.GradeID = Guid.NewGuid();
        }

        public Grade(Student student, Test test)
        {
            this.Student = student;
            this.Test = test;
            this.GradeID = Guid.NewGuid();
        }

        public Grade(Student student, Test test, int points)
        {
            this.Student = student;
            this.Test = test;
            this.Points = points;
            this.GradeID = Guid.NewGuid();
        }
        #endregion  

        public Guid GradeID { get; private set; }
        public int Points { get; set; }
        public Test Test { get; set; }
        public Student Student { get; set; }

        //public List<Grade> crossJoinLists(List<Student> List1, List<Test> List2)
        //{
        //    //int grade = 0;
        //    //List<int> grades = new List<int>(grade);

        //    var crossJoin = (from a in List1
        //                     from b in List2
        //                         //from c in grades
        //                     select new
        //                     {
        //                         Name = a.Name,
        //                         Nickname = a.Nickname,
        //                         Subject = b.Subject,
        //                         TestNumber = b.SeriesNumber,
        //                         Grades = 0
        //                     }).ToList();

        //    //Anonymous types cannot be edited in datagridview. Need to add results to a List<Grade> so we can edit the grades. 
        //    foreach (var item in crossJoin)
        //    {
        //        //first check if the entry already exists
        //        int inList = -1;

        //        inList = grades.FindIndex(g => g.Name + "||" + g.Subject + "||" + g.TestNumber == item.Name + "||" + item.Subject + "||" + item.TestNumber);

        //        if (inList == -1)
        //        {
        //            grades.Add(new Grade()
        //            {
        //                Name = item.Name
        //                ,
        //                Nickname = item.Nickname
        //                ,
        //                Subject = item.Subject
        //                ,
        //                TestNumber = item.TestNumber
        //                ,
        //                Grades = item.Grades
        //            }
        //            );
        //        }
        //    }

        //    return grades;

        //}

    }
}



