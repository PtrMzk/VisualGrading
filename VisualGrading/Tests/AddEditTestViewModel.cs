﻿using System;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.ViewModelHelpers;

namespace VisualGrading.Tests
{
    public class AddEditTestViewModel : BaseViewModel
    {
        #region Fields

        private readonly IBusinessManager _businessManager;

        private Test _editingTest;

        private bool _editMode;

        private SimpleEditableTest _test;

        #endregion

        #region Constructors

        public AddEditTestViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        public SimpleEditableTest EditingTest
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

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

            destination.Subject = source.Subject;
            destination.SeriesNumber = source.SeriesNumber;

            destination.Date = EditMode ? source.Date : DateTime.Now;

            destination.Name = source.Name;
            destination.SubCategory = source.SubCategory;
            destination.MaximumPoints = source.MaximumPoints;
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

        public event Action Done = delegate { };
    }
}