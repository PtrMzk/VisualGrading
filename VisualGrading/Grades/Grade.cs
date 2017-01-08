using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Annotations;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Grades
{
    [Serializable]
    public class Grade : INotifyPropertyChanged
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
            this.StudentID = student.ID;
            this.Test = test;
            this.TestID = test.ID;
            this.Points = points;
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(long studentID, long testID)
        {
            this.StudentID = studentID;
            this.TestID = testID;
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(long studentID, long testID, int points)
        {
            this.StudentID = studentID;
            this.TestID = testID;
            this.Points = points;
            //this.GradeID = Guid.NewGuid();
        }
        #endregion

        #region Properties
        public long ID { get; set; }

        public int Points
        {
            get { return _points; }
            set
            {
                if (value == _points) return;
                _points = value;
                OnPropertyChanged();
            }
        }

        public long StudentID { get; set; }
        public long TestID { get; set; }

        [NonSerialized]
        private Test _test;

        public Test Test
        {
            get { return _test; }
            set { _test = value; }
        }

        [NonSerialized]
        private Student _student;

        private int _points;

        public Student Student
        {
            get { return _student; }
            set { _student = value; }
        }

        public decimal PercentAverage
        {
            get { return _points / (decimal) (Test.MaximumPoints == 0 ? 1 : Test.MaximumPoints); }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



