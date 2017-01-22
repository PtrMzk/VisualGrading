using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using StudentTestReporting.Annotations;

namespace VisualGrading.Students
{
    [Serializable]
    public class Student : INotifyPropertyChanged
    {
        private string _emailAddress;
        private string _nickname;
        private string _firstName;
        private string _lastName;
        private string _parentEmailAddress;

        public Student()
        {
            //this.ID = Guid.NewGuid();
        }

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

        public string FullName { get { return string.Format("{0} {1}", !string.IsNullOrEmpty(Nickname) ? Nickname : FirstName, LastName); }}

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

        #region StudentRefreshEvent
        //TODO: is this event needed? 
        //public delegate void StudentRefreshEventHandler(object sender, EventArgs args);

        //public event StudentRefreshEventHandler StudentRefreshing;

        //protected virtual void OnStudentRefreshing()
        //{
        //    if (StudentRefreshing != null)
        //    {
        //        StudentRefreshing(this, EventArgs.Empty);
        //    }
        //}
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
