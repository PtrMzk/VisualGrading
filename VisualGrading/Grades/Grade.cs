using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudentTestReporting.Annotations;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Grades
{
    [Serializable]
    public class Grade : INotifyPropertyChanged
    {
        #region Fields

        private int? _points;

        [NonSerialized] private Student _student;

        [NonSerialized] private Test _test;

        #endregion

        #region Constructors

        public Grade()
        {
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(Student student, Test test)
        {
            Student = student;
            Test = test;
            // this.GradeID = Guid.NewGuid();
        }

        public Grade(Student student, Test test, int points)
        {
            Student = student;
            StudentID = student.ID;
            Test = test;
            TestID = test.ID;
            Points = points;
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(long studentID, long testID)
        {
            StudentID = studentID;
            TestID = testID;
            //this.GradeID = Guid.NewGuid();
        }

        public Grade(long studentID, long testID, int points)
        {
            StudentID = studentID;
            TestID = testID;
            Points = points;
            //this.GradeID = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public long ID { get; set; }

        public int? Points
        {
            get { return _points; }
            set
            {
                if (value == _points) return;
                _points = value;
                OnPropertyChanged();
            }
        }

        //used for calculations. calling code should always check for nulls in the Points property first.
        public int NonNullablePoints
        {
            get
            {
                if (_points == null) return 0;
                return (int)_points;
            }
        }

        public long StudentID { get; set; }
        public long TestID { get; set; }

        public Test Test
        {
            get { return _test; }
            set { _test = value; }
        }

        public Student Student
        {
            get { return _student; }
            set { _student = value; }
        }

        public decimal PercentAverage
        {
            get
            {
                if (_points != null)
                    return (int)_points / (decimal) (Test.MaximumPoints == 0 ? 1 : Test.MaximumPoints);

                return 0m;
            }
        }

        #endregion

        #region Private Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Interface Implementations

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}