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
           // this.GradeID = Guid.NewGuid();
        }

        public Grade(Student student, Test test, int points)
        {
            this.Student = student;
            this.Test = test;
            this.Points = points;
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(int studentID, int testID)
        {
            this.StudentID = studentID;
            this.TestID = testID;
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(int studentID, int testID, int points)
        {
            this.StudentID = studentID;
            this.TestID = testID;
            this.Points = points;
            //this.GradeID = Guid.NewGuid();
        }
        #endregion

        #region Properties
        public int GradeID { get; set; }
        public int Points { get; set; }
        public int StudentID { get; set; }
        public int TestID { get; set; }

        [NonSerialized]
        private Test _test;

        public Test Test
        {
            get { return _test; }
            set { _test = value; }
        }

        [NonSerialized]
        private Student _student;
        public Student Student
        {
            get { return _student; }
            set { _student = value; }
        }
        #endregion 
    }
}



