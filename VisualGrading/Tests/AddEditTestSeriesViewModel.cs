using System;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Tests
{
    public class AddEditTestSeriesViewModel : BaseViewModel
    {
        #region Constructor

        public AddEditTestSeriesViewModel(ITestRepository repository)
        {
            _repository = repository;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        private readonly ITestRepository _repository;

        private TestSeries _editingTestSeries;

        public SimpleEditableTestSeries EditingTestSeries
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
            if (EditingTestSeries != null) EditingTestSeries.ErrorsChanged -= RaiseCanExecuteChanged;
            EditingTestSeries = new SimpleEditableTestSeries();
            EditingTestSeries.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTestSeries(testSeries, EditingTestSeries);
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
            return !EditingTestSeries.HasErrors;
        }

        private async void OnSave()
        {
            UpdateTestSeries(EditingTestSeries, _editingTestSeries);
                _repository.AddTestSeriesAsync(_editingTestSeries);
            Done();
        }

        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}