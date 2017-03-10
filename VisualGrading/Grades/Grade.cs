using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using StudentTestReporting.Annotations;
using VisualGrading.Helpers;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Grades
{
    [Serializable]
    public class Grade : INotifyPropertyChanged, IIdentified
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
                return (int) _points;
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
                    return (int) _points / (decimal) (Test.MaximumPoints == 0 ? 1 : Test.MaximumPoints);

                return 0m;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var property in typeof(Grade).GetProperties())
            {
                var propertyValue = property.GetValue(this);

                if (stringBuilder.Length == 0)
                    stringBuilder.Append(propertyValue);
                else
                    stringBuilder.AppendFormat(" {0}", propertyValue);
            }
            return stringBuilder.ToString();
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