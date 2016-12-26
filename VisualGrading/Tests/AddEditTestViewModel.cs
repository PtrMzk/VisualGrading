using System;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Tests
{
    public class AddEditTestViewModel : BaseViewModel
    {
        #region Constructor

        public AddEditTestViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties
       

        private IBusinessManager _businessManager;


        private Test _editingTest;

        public SimpleEditableTest EditingTest
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
            if (EditingTest != null) EditingTest.ErrorsChanged -= RaiseCanExecuteChanged;
            EditingTest = new SimpleEditableTest();
            EditingTest.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTest(test, EditingTest);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void UpdateTest(SimpleEditableTest source, Test destination)
        {
            destination.Subject = source.Subject;
            destination.SeriesNumber = source.SeriesNumber;
            destination.Date = source.Date;
            destination.Name = source.Name;
            destination.SubCategory = source.SubCategory;
            destination.MaximumPoints = source.MaximumPoints;
        }

        private void CopyTest(Test source, SimpleEditableTest destination)
        {
            destination.ID = source.ID;

            if (EditMode)
            {
                destination.Subject = source.Subject;
                destination.SeriesNumber = source.SeriesNumber;
                destination.Date = source.Date;
                destination.Name = source.Name;
                destination.SubCategory = source.SubCategory;
                destination.MaximumPoints = source.MaximumPoints;
            }
        }

        private bool CanSave()
        {
            return !EditingTest.HasErrors;
        }

        private async void OnSave()
        {
            UpdateTest(EditingTest, _editingTest);

            if (EditMode)
               await _businessManager.UpdateTestAsync(_editingTest);

            else
                await _businessManager.InsertTestAsync(_editingTest);

            Done();
        }

        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}