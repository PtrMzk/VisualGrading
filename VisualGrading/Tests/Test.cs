using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudentTestReporting.Annotations;

namespace VisualGrading.Tests
{
    public class Test : INotifyPropertyChanged
    {
        private DateTime _date;
        private int _maximumPoints;
        private string _name;
        private int _seriesNumber;
        private string _subCategory;
        private string _subject;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}