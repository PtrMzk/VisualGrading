#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// Test.cs
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

#endregion

namespace VisualGrading.Tests
{
    public class Test : INotifyPropertyChanged, IIdentified
    {
        #region Fields

        private DateTime _date;
        private int _maximumPoints;
        private string _name;
        private int _seriesNumber;
        private string _subCategory;
        private string _subject;

        #endregion

        #region Constructors

        public Test()
        {
            //this.TestID = Guid.NewGuid();
        }

        public Test(string name, string subject, string subCategory, int seriesNumber, int maxPoints)
        {
            //this.tID = Guid.NewGuid();
            Name = name;
            Subject = subject;
            SubCategory = subCategory;
            SeriesNumber = seriesNumber;
            MaximumPoints = maxPoints;
        }

        #endregion

        #region Properties

        public long ID { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Subject
        {
            get { return _subject; }
            set
            {
                if (value == _subject) return;
                _subject = value;
                OnPropertyChanged();
            }
        }

        public string SubCategory
        {
            get { return _subCategory; }
            set
            {
                if (value == _subCategory) return;
                _subCategory = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Equals(_date)) return;
                _date = value;
                OnPropertyChanged();
            }
        }

        public int SeriesNumber
        {
            get { return _seriesNumber; }
            set
            {
                if (value == _seriesNumber) return;
                _seriesNumber = value;
                OnPropertyChanged();
            }
        }

        public int MaximumPoints
        {
            get { return _maximumPoints; }
            set
            {
                if (value == _maximumPoints) return;
                _maximumPoints = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var property in typeof(Test).GetProperties())
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