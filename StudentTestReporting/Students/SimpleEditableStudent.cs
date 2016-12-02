using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Helpers;

namespace StudentTestReporting.Students
{
    public class SimpleEditableStudent : ValidatableBaseViewModel
    {
        #region Public Properties
        [Required]
        public Guid StudentID
        {
            get { return _studentID; }
            set { SetProperty(ref _studentID, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { SetProperty(ref _emailAddress, value); }
        }

        public string ParentEmailAddress
        {
            get { return _parentEmailAddress; }
            set { SetProperty(ref _parentEmailAddress, value); }
        }

        #endregion

        #region Private Properties

        private Guid _studentID;
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _parentEmailAddress;
        
        #endregion
    }
}
