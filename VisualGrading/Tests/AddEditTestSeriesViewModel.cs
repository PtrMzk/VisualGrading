using System;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.ViewModelHelpers;

namespace VisualGrading.Tests
{
    public class AddEditTestSeriesViewModel : BaseViewModel
    {
        #region Constructor

        public AddEditTestSeriesViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties
        
        private TestSeries _editingTestSeries;

        private IBusinessManager _businessManager;


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
            destination.ID = source.ID;
            destination.Name = source.Name;
            destination.Subject = source.Subject;
            destination.SubCategory = source.SubCategory;
            destination.TestCount = source.Length;
            destination.MaximumPoints = source.MaximumPoints; 

        }

        private void CopyTestSeries(TestSeries source, SimpleEditableTestSeries destination)
        {
            destination.ID = source.ID;
            destination.Name = source.Name;
            destination.Subject = source.Subject;
            destination.SubCategory = source.SubCategory;
            destination.Length = source.TestCount;
            destination.MaximumPoints = source.MaximumPoints;
        }

        private bool CanSave()
        {
            return !EditingTestSeries.HasErrors;
        }

        private async void OnSave()
        {
            UpdateTestSeries(EditingTestSeries, _editingTestSeries);
            await _businessManager.InsertTestSeriesAsync(_editingTestSeries);
            Done();
        }
        
        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}