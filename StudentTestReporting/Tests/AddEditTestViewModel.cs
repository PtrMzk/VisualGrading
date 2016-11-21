using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using StudentTestReporting.GraphicFrontEnd;

namespace StudentTestReporting.Tests
{
    public class AddEditTestViewModel : StudentTestReporting.GraphicFrontEnd.BaseViewModel
    {
        #region Constructor

        public AddEditTestViewModel()
        {

        }

        #endregion

        #region Properties

        public AddEditMode Mode
        {
            get { return _mode; }
            set { SetProperty(ref _mode, value); }
        }

        private Test _editingTest = null;

        public SimpleEditableTest Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        private SimpleEditableTest _test;
        private AddEditMode _mode;

        public enum AddEditMode
        {
            AddMode,
            EditMode
        }

        #endregion

        #region Methods

        public void SetTest(Test test)
        {
            _editingTest = test;
            Test = new SimpleEditableTest();
            CopyTest(test, Test);
        }

        private void CopyTest(Test source, SimpleEditableTest target)
        {
            target.TestID = source.TestID;

            if (Mode == AddEditMode.EditMode)
            {
                target.Subject = source.Subject;
                target.TestNumber = source.TestNumber;
            }
        }

        #endregion

    }
}
