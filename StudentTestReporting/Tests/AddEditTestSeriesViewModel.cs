using System;
using StudentTestReporting.Helpers;
using StudentTestReporting.Presentation;

namespace StudentTestReporting.Tests
{
    public class AddEditTestSeriesViewModel : BaseViewModel
    {
        #region Constructor

        public AddEditTestSeriesViewModel(ITestManager manager)
        {
            _manager = manager;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        private readonly ITestManager _manager;

        private TestSeries _editingTestSeries;

        public SimpleEditableTestSeries TestSeries
        {
            get { return _testSeries; }
            set { SetProperty(ref _testSeries, value); }
        }

        private SimpleEditableTestSeries _testSeries;

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

        public event Action Done = delegate { };

        #endregion

        #region Methods

        public void SetTestSeries(TestSeries testSeries)
        {
            _editingTestSeries = testSeries;
            if (TestSeries != null) TestSeries.ErrorsChanged -= RaiseCanExecuteChanged;
            TestSeries = new SimpleEditableTestSeries();
            TestSeries.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTestSeries(testSeries, TestSeries);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void UpdateTestSeries(SimpleEditableTestSeries source, TestSeries destination)
        {
            destination.SeriesID = source.SeriesID;
            destination.Name = source.Name;
            destination.Subject = source.Subject;
            destination.SubCategory = source.SubCategory;
            destination.Length = source.Length;

        }

        private void CopyTestSeries(TestSeries source, SimpleEditableTestSeries destination)
        {
            destination.SeriesID = source.SeriesID;
            destination.Name = source.Name;
            destination.Subject = source.Subject;
            destination.SubCategory = source.SubCategory;
            destination.Length = source.Length;
        }

        private bool CanSave()
        {
            return !TestSeries.HasErrors;
        }

        private async void OnSave()
        {
            UpdateTestSeries(TestSeries, _editingTestSeries);
                _manager.AddTestSeriesAsync(_editingTestSeries);
            Done();
        }

        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}