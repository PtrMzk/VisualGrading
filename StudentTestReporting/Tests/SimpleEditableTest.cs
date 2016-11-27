using StudentTestReporting.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Helpers;

namespace StudentTestReporting.Tests
{
    public class SimpleEditableTest : ValidatableBaseViewModel
    {

        #region Public Properties
        [Required]
        public Guid TestID
        {
            get { return _testID; } 
            set { SetProperty(ref _testID, value); }
        }

        [Required]
        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

        public int TestNumber 
        {
            get { return _testNumber; }
            set { SetProperty(ref _testNumber, value); }
        }
        #endregion

        #region Private Properties

        private Guid _testID;
        private string _subject;
        private int _testNumber;

        #endregion
    }
}
