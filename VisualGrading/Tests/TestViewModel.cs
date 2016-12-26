using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Tests
{
    public class TestViewModel : BaseViewModel
    {
        #region Constructor

        public TestViewModel()
        {

            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            DeleteCommand = new RelayCommand<Test>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddTest);
            AddSeriesCommand = new RelayCommand(OnAddTestSeries);
            EditCommand = new RelayCommand<Test>(OnEditTest);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            DeleteRequested += DeleteTest;

            //TODO: delete the below if not needed, clean this up to set the private variable test
            //var _observableTests = new ObservableCollection<TestDTO>();
            //this._observableTests = new ObservableCollection<TestDTO>();

            //this._observableTests.AddTest(new TestDTO()
            //{
            //    Subject = "Computer Programming",
            //    TestNumber = 1
            //});
            //this._observableTests.AddTest(new TestDTO()
            //{

            //    Subject = "Computer Programming",
            //    TestNumber = 2
            //});
            //this._observableTests.AddTest(new TestDTO()
            //{

            //    Subject = "Science",
            //    TestNumber = 1
            //});
            //PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
        }

        #endregion

        #region Properties


        private IBusinessManager _businessManager;


        private ObservableCollectionExtended<Test> _observableTests;

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


        private List<Test> _allTests;

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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        public event Action<Test> AddRequested = delegate { };
        public event Action<TestSeries> AddSeriesRequested = delegate { };
        public event Action<Test> EditRequested = delegate { };
        public event Action<Test> DeleteRequested = delegate { };

        private string _searchInput;

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

        #region Methods

        public async void LoadTests()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            if (_allTests == null)
            {
                _allTests = await _businessManager.GetTestsAsync();
                ObservableTests = new ObservableCollectionExtended<Test>(_allTests);
                //if (ObservableTests == null || ObservableTests.Count == 0)
                //{
                //    TestDTO test = new TestDTO();
                //    test.TestList();
                //    ObservableTests.AddTest(test);

                PropertyChanged(this, new PropertyChangedEventArgs("ObservableTests"));
            }
            //}
        }

        private void FilterTests(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
                ObservableTests = new ObservableCollectionExtended<Test>(_allTests);
            else
                ObservableTests =
                    new ObservableCollectionExtended<Test>(
                        _allTests.Where(t => t.Subject.ToLower().Contains(searchInput.ToLower())));
        }

        private async void DeleteTest(Test test)
        {
            ObservableTests.Remove(test);
            _allTests.Remove(test);
            await _businessManager.DeleteTestAsync(test);
        }

        private void OnDelete(Test test)
        {
            DeleteRequested(test);
        }

        private async void ObservableTests_CollectionChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            await _businessManager.UpdateTestAsync((Test)sender);
        }

        //TODO: RemoveTest below method - its a temp method for the AddTest TestDTO > Charting workflow
        //private void OnAddTest(TestDTO test)
        //{
        //    test.TestNumber += 1;
        //    PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
        //    AddTestRequested(test);
        //}

        private void OnAddTest()
        {
            //place holder for the actual on add test command for the actual on add test button
            //the one above is linked to the chart button i believe...
            AddRequested(new Test());
        }

        private void OnAddTestSeries()
        {
            AddSeriesRequested(new TestSeries());
        }

        private void OnEditTest(Test test)
        {
            EditRequested(test);
        }

        //FIXME: THIS IS NEVER FALSE
        private bool CanDelete(Test test)
        {
            //TODO: Selected test doesn't seem to work here, and this isn't really needed...
            //return SelectedTest != null;
            return true;
        }

        private void OnClearSearch()
        {
            SearchInput = null;
        }

        #endregion
    }
}