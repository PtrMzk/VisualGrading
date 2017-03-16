#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// TestViewModel.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Presentation;
using VisualGrading.Search;

#endregion

namespace VisualGrading.Tests
{
    public class TestViewModel : BaseViewModel
    {
        #region Fields

        private readonly IBusinessManager _businessManager;

        private List<Test> _allTests;

        private ObservableCollectionExtended<Test> _observableTests;

        private string _searchInput;

        #endregion

        #region Constructors

        public TestViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            DeleteCommand = new RelayCommand<Test>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddTest);
            AddSeriesCommand = new RelayCommand(OnAddTestSeries);
            EditCommand = new RelayCommand<Test>(OnEditTest);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            ChartTestCommand = new RelayCommand<Test>(OnChartRequested);
            ChartSubjectCommand = new RelayCommand<string>(OnSubjectChartRequested);
            ChartSubCategoryCommand = new RelayCommand<string>(OnSubCategoryChartRequested);
            GoToTestGradesCommand = new RelayCommand<long>(OnGoToTestGradesRequested);

            DeleteRequested += DeleteTest;
        }

        #endregion

        #region Properties

        public ObservableCollectionExtended<Test> ObservableTests
        {
            get { return _observableTests; }
            set
            {
                if (_observableTests != null && value != _observableTests)
                    _observableTests.CollectionPropertyChanged -= ObservableTests_CollectionChanged;

                SetProperty(ref _observableTests, value);
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableTests"));
                _observableTests.CollectionPropertyChanged += ObservableTests_CollectionChanged;
            }
        }

        private Test _selectedTest { get; set; }

        public Test SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                if (_selectedTest != value)
                {
                    _selectedTest = value;
                    DeleteCommand.RaiseCanExecuteChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectTest"));
                }
            }
        }

        public RelayCommand<Test> DeleteCommand { get; }

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Test> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public RelayCommand<Test> ChartTestCommand { get; private set; }

        public RelayCommand<long> GoToTestGradesCommand { get; private set; }

        public RelayCommand<string> ChartSubjectCommand { get; private set; }

        public RelayCommand<string> ChartSubCategoryCommand { get; private set; }

        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterTests(_searchInput);
            }
        }

        #endregion

        #region Public Methods

        public void ClearSearch()
        {
            SearchInput = null;
        }

        public async void LoadTests()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _allTests = await _businessManager.GetTestsAsync();
            ObservableTests = new ObservableCollectionExtended<Test>(_allTests);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableTests"));

            //reapply search filter
            FilterTests(SearchInput);
        }

        public void SearchTests(string searchInput)
        {
            SearchInput = searchInput;
        }

        #endregion

        #region Private Methods

        //TODO: THIS IS NEVER FALSE
        private bool CanDelete(Test test)
        {
            //TODO: Selected test doesn't seem to work here, and this isn't really needed...
            //return SelectedTest != null;
            return true;
        }

        private async void DeleteTest(Test test)
        {
            ObservableTests.Remove(test);
            _allTests.Remove(test);
            await _businessManager.DeleteTestAsync(test);
        }

        private void FilterTests(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableTests = new ObservableCollectionExtended<Test>(_allTests);
            }
            else
            {
                var smartSearch = new SmartSearch<Grade>();
                var matchingIDs = smartSearch.Search(_allTests, searchInput);

                ObservableTests =
                    new ObservableCollectionExtended<Test>(
                        _allTests.Where(g => matchingIDs.Contains(g.ID)));
            }
        }

        private async void ObservableTests_CollectionChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            await _businessManager.UpdateTestAsync((Test) sender);
        }

        private void OnAddTest()
        {
            AddRequested(new Test());
        }

        private void OnAddTestSeries()
        {
            AddSeriesRequested(new TestSeries());
        }

        private void OnChartRequested(Test grouping)
        {
            TestChartRequested(grouping);
        }

        private void OnClearSearch()
        {
            ClearSearch();
        }

        private void OnDelete(Test test)
        {
            DeleteRequested(test);
        }

        private void OnEditTest(Test test)
        {
            EditRequested(test);
        }

        private void OnGoToTestGradesRequested(long id)
        {
            GoToTestGradesRequested(id);
        }

        private void OnSubCategoryChartRequested(string grouping)
        {
            SubCategoryChartRequested(grouping);
        }

        private void OnSubjectChartRequested(string grouping)
        {
            SubjectChartRequested(grouping);
        }

        #endregion

        public event Action<Test> AddRequested = delegate { };
        public event Action<TestSeries> AddSeriesRequested = delegate { };
        public event Action<Test> DeleteRequested = delegate { };
        public event Action<Test> EditRequested = delegate { };
        public event Action<long> GoToTestGradesRequested = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Action<string> SubCategoryChartRequested = delegate { };
        public event Action<string> SubjectChartRequested = delegate { };
        public event Action<Test> TestChartRequested = delegate { };
    }
}