#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// AddEditTestViewModel.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

#endregion

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

        #region Public Methods

        public void SetTest(Test test)
        {
            _editingTest = test;
            if (EditingTest != null) EditingTest.ErrorsChanged -= RaiseCanExecuteChanged;
            EditingTest = new SimpleEditableTest();
            EditingTest.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTest(test, EditingTest);
        }

        #endregion

        #region Private Methods

        private bool CanSave()
        {
            return !EditingTest.HasErrors;
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

        private void OnCancel()
        {
            Done();
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

        #endregion

        public event Action Done = delegate { };
    }
}