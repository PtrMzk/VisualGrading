using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using StudentTestReporting.Annotations;
using VisualGrading.Helpers;

namespace VisualGrading.Students
{
    [Serializable]
    public class Student : INotifyPropertyChanged, IIdentified
    {
        #region Fields

        private string _emailAddress;
        private string _firstName;
        private string _lastName;
        private string _nickname;
        private string _parentEmailAddress;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public long ID { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName) return;
                _firstName = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", !string.IsNullOrEmpty(Nickname) ? Nickname : FirstName, LastName); }
        }

        public string Nickname
        {
            get { return _nickname; }
            set
            {
                if (value == _nickname) return;
                _nickname = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(FullName));
            }
        }

        [EmailAddress]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (value == _emailAddress) return;
                _emailAddress = value;

                OnPropertyChanged();
            }
        }

        [EmailAddress]
        public string ParentEmailAddress
        {
            get { return _parentEmailAddress; }
            set
            {
                if (value == _parentEmailAddress) return;
                _parentEmailAddress = value;
                OnPropertyChanged();
            }
        }

        public decimal OverallGrade { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var property in typeof(Student).GetProperties())
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

        //}
        //    }
        //        StudentRefreshing(this, EventArgs.Empty);
        //    {
        //    if (StudentRefreshing != null)
        //{

        //protected virtual void OnStudentRefreshing()

        //public event StudentRefreshEventHandler StudentRefreshing;
        //public delegate void StudentRefreshEventHandler(object sender, EventArgs args);
        //TODO: is this event needed? 
    }
}