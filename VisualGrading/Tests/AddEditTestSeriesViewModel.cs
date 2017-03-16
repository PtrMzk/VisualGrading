#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// AddEditTestSeriesViewModel.cs
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
    public class AddEditTestSeriesViewModel : BaseViewModel
    {
        #region Fields

        private readonly IBusinessManager _businessManager;

        private TestSeries _editingTestSeries;

        private SimpleEditableTestSeries _testSeries;

        #endregion

        #region Constructors

        public AddEditTestSeriesViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        public SimpleEditableTestSeries EditingTestSeries
        {
            get { return _testSeries; }
            set { SetProperty(ref _testSeries, value); }
        }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

        #endregion

        #region Public Methods

        public void SetTestSeries(TestSeries testSeries)
        {
            _editingTestSeries = testSeries;
            if (EditingTestSeries != null) EditingTestSeries.ErrorsChanged -= RaiseCanExecuteChanged;
            EditingTestSeries = new SimpleEditableTestSeries();
            EditingTestSeries.ErrorsChanged += RaiseCanExecuteChanged;
            CopyTestSeries(testSeries, EditingTestSeries);
        }

        #endregion

        #region Private Methods

        private bool CanSave()
        {
            return !EditingTestSeries.HasErrors;
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

        private void OnCancel()
        {
            Done();
        }

        private async void OnSave()
        {
            UpdateTestSeries(EditingTestSeries, _editingTestSeries);
            await _businessManager.InsertTestSeriesAsync(_editingTestSeries);
            Done();
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

        #endregion

        public event Action Done = delegate { };
    }
}