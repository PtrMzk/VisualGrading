#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-18
// Grade.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using StudentTestReporting.Annotations;
using VisualGrading.Helpers;
using VisualGrading.Students;
using VisualGrading.Tests;

#endregion

namespace VisualGrading.Grades
{
    [Serializable]
    public class Grade : INotifyPropertyChanged, IIdentified
    {
        #region Fields

        private const string ID_CONSTANT = "ID";
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
                OnPropertyChanged("PercentAverage");
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
                if (property.Name.ToUpper().EndsWith(ID_CONSTANT)) //skip ID properties
                    continue;

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