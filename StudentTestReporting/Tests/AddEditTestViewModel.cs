using System;
using StudentTestReporting.Helpers;
using StudentTestReporting.Presentation;

namespace StudentTestReporting.Tests
{
    public class AddEditTestViewModel : BaseViewModel
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

        private readonly ITestManager _manager;

        private Test _editingTest;

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

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

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

        private void UpdateTest(SimpleEditableTest source, Test destination)
        {
            destination.Subject = source.Subject;
            destination.TestID = source.TestID;
            destination.TestNumber = source.TestNumber;
        }

        private void CopyTest(Test source, SimpleEditableTest target)
        {
            target.TestID = source.TestID;

            if (EditMode)
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
            UpdateTest(Test, _editingTest);
            if (EditMode)
                _manager.UpdateTestAsync(_editingTest);
            else
                _manager.AddTestAsync(_editingTest);
            Done();
        }

        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}