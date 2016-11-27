using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using StudentTestReporting.Presentation;
using StudentTestReporting.Helpers;

namespace StudentTestReporting.Tests
{
    public class AddEditTestViewModel : StudentTestReporting.Presentation.BaseViewModel
    {
        #region Constructor

        public AddEditTestViewModel(ITestManager manager)
        {
            _manager = manager; 
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        private ITestManager _manager;

        private Test _editingTest = null;

        public SimpleEditableTest Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        private SimpleEditableTest _test;

        private bool _editMode;

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public RelayCommand CancelCommand
        {
            get; private set;
        }

        public RelayCommand SaveCommand
        {
            get; private set;
        }

        public event Action Done = delegate { };

        #endregion

        #region Methods

        public void SetTest(Test test)
        {
            _editingTest = test;
            if (Test != null) Test.ErrorsChanged -= RaiseCanExecuteChanged;
            Test = new SimpleEditableTest();
            Test.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTest(test, Test);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void CopyTest(Test source, SimpleEditableTest target)
        {
            target.TestID = source.TestID;

            if (EditMode == true)
            {
                target.Subject = source.Subject;
                target.TestNumber = source.TestNumber;
            }
        }

        private bool CanSave()
        {
            return !Test.HasErrors;
        }

        private async void OnSave()
        {
            Done();
        }

        private void OnCancel()
        {
            Done();

        }
        #endregion

    }
}
