using StudentTestReporting.GraphicFrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.Tests
{
    public class SimpleEditableTest : BaseViewModel
    {
        public Guid TestID
        {
            get { return _testID; } 
            set { SetProperty(ref _testID, value); }
        }

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

        #region Public Properties
        
        #endregion

        #region Private Properties

        private Guid _testID;
        private string _subject;
        private int _testNumber;

        #endregion
    }
}
