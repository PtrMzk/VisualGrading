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
    public class Grades : List<Grade> { }
    [Serializable]
    public class Grade
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Subject { get; set; }
        public int TestNumber { get; set; }
        private int gradesSafe;
        public int Grades
        {
            get { return gradesSafe; }
            set
            {
                if (value > 100)
                    gradesSafe = 100;
                else if (value < 0)
                    gradesSafe = 0;
                else
                    gradesSafe = value;
            }
        }

        List<Grade> grades = new List<Grade>();

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



