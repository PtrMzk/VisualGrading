using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using VisualGrading.Tests;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Tests
{
    public class TestViewModel : BaseViewModel
    {
        #region Constructor

        public TestViewModel(ITestManager manager)
        {
            _manager = manager;
            DeleteCommand = new RelayCommand<Test>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddTest);
            AddSeriesCommand = new RelayCommand(OnAddTestSeries);
            EditCommand = new RelayCommand<Test>(OnEditTest);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            DeleteRequested += RemoveTestFromPresentationAndManager; 

            //TODO: delete the below if not needed, clean this up to set the private variable test
            //var _observableTests = new ObservableCollection<Test>();
            //this._observableTests = new ObservableCollection<Test>();

            //this._observableTests.AddTest(new Test()
            //{
            //    Subject = "Computer Programming",
            //    TestNumber = 1
            //});
            //this._observableTests.AddTest(new Test()
            //{

            //    Subject = "Computer Programming",
            //    TestNumber = 2
            //});
            //this._observableTests.AddTest(new Test()
            //{

            //    Subject = "Science",
            //    TestNumber = 1
            //});
            //PropertyChanged(this, new PropertyChangedEventArgs("Tests"));

        }

        #endregion

        #region Properties

        private ITestManager _manager;

        private ObservableCollection<Test> _observableTests;

        public ObservableCollection<Test> ObservableTests
        {
            get
            {
                return _observableTests;
            }
            set
            {
                SetProperty(ref _observableTests, value);
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableTests"));
            }
        }

        private List<Test> _allTests;

        private Test _selectedTest { get; set; }

        public Test SelectedTest
        {
            get
            {
                return _selectedTest;
            }
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

        public RelayCommand<Test> DeleteCommand { get; private set; }

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
            get
            {
                return _searchInput;

            }
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
               new System.Windows.DependencyObject())) return;
            //var ObservableTests = new List<Test>();
            _allTests =
                await
                    _manager.GetTestsAsync();
            ObservableTests = new ObservableCollection<Test>(_allTests);
            //if (ObservableTests == null || ObservableTests.Count == 0)
            //{
            //    Test test = new Test();
            //    test.TestList();
            //    ObservableTests.AddTest(test);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableTests"));
            //}
        }

        private void FilterTests(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableTests = new ObservableCollection<Test>(_allTests);
                return;
            }
            else
            {
                ObservableTests = new ObservableCollection<Test>(_allTests.Where(t => t.Subject.ToLower().Contains(searchInput.ToLower())));
            }
        }

        private void RemoveTestFromPresentationAndManager(Test test)
        {
            ObservableTests.Remove(test);
            _manager.RemoveTest(test);
        }

        private void OnDelete(Test test)
        {
            DeleteRequested(test);
        }

        //TODO: RemoveTest below method - its a temp method for the AddTest Test > Charting workflow
        //private void OnAddTest(Test test)
        //{
        //    test.TestNumber += 1;
        //    PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
        //    AddTestRequested(test);
        //}

        private void OnAddTest()
        {
            //place holder for the actual on add test command for the actual on add test button
            //the one above is linked to the chart button i believe...
            AddRequested(new Test() {TestID = Guid.NewGuid()});
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
