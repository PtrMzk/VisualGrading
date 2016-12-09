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
            //this.GradeID = Guid.NewGuid();
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

        public Guid GradeID { get; set; }
        public int Points { get; set; }
        public Test Test { get; set; }
        public Student Student { get; set; }
    }
}



