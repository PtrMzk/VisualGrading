using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using StudentTestReporting.Tests;
using StudentTestReporting.Helpers;
using StudentTestReporting.Presentation;

namespace StudentTestReporting.Tests
{
    public class TestViewModel : StudentTestReporting.Presentation.BaseViewModel
    {

        #region Singleton Implementation
        //TODO: Is Singleton even needed here? 
        //static readonly TestViewModel instance = new TestViewModel(ITestManager);

        //static TestViewModel()
        //{

        //}

        //public static TestViewModel Instance
        //{
        //    get
        //    {
        //        return instance;
        //    }
        //}
        #endregion

        #region Constructor

        public TestViewModel(ITestManager manager)
        {
            _manager = manager;
            DeleteTestCommand = new RelayCommand(OnDelete, CanDelete);
            AddTestCommand = new RelayCommand(OnAddTest);
            EditTestCommand = new RelayCommand<Test>(OnEditTest);
            ClearSearchCommand = new RelayCommand(OnClearSearch);


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
                //PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
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
                    DeleteTestCommand.RaiseCanExecuteChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
                }
            }
        }

        public RelayCommand DeleteTestCommand { get; private set; }

        public RelayCommand AddTestCommand { get; private set; }

        public RelayCommand<Test> EditTestCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

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
                    _manager.GetTestsAsync(
                        @"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\test.json");
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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnDelete()
        {
            ObservableTests.Remove(SelectedTest);
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
            AddTestRequested(new Test { TestID = Guid.NewGuid() });
        }

        private void OnEditTest(Test test)
        {
            EditTestRequested(test);
        }

        public event Action<Test> AddTestRequested = delegate { };
        public event Action<Test> EditTestRequested = delegate { };


        //FIXME: THIS IS NEVER FALSE
        private bool CanDelete()
        {
            return SelectedTest != null;
        }

        private void OnClearSearch()
        {
            SearchInput = null;
        }

        #endregion
    }
}
