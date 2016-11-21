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
using StudentTestReporting.GraphicFrontEnd;

namespace StudentTestReporting.Tests
{
    public class TestViewModel : StudentTestReporting.GraphicFrontEnd.BaseViewModel
    {

        #region Singleton Implementation

        static readonly TestViewModel instance = new TestViewModel();

        static TestViewModel()
        {

        }

        public static TestViewModel Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region Constructor

        public TestViewModel()
        {
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
            AddTestCommand = new RelayCommand(OnAddTest);
            EditTestCommand = new RelayCommand<Test>(OnEditTest);

            //TODO: delete the below if not needed, clean this up to set the private variable test
            //var _tests = new ObservableCollection<Test>();
            //this._tests = new ObservableCollection<Test>();

            //this._tests.Add(new Test()
            //{
            //    Subject = "Computer Programming",
            //    TestNumber = 1
            //});
            //this._tests.Add(new Test()
            //{

            //    Subject = "Computer Programming",
            //    TestNumber = 2
            //});
            //this._tests.Add(new Test()
            //{

            //    Subject = "Science",
            //    TestNumber = 1
            //});
            //PropertyChanged(this, new PropertyChangedEventArgs("Tests"));

        }

        #endregion

        #region Properties

        private TestManager _TestManager = new TestManager();

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
                    PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
                }
            }
        }

        public RelayCommand DeleteCommand { get; private set; }

        public RelayCommand AddTestCommand { get; private set; }

        public RelayCommand<Test> EditTestCommand { get; private set; }

        private ObservableCollection<Test> _tests;

        public ObservableCollection<Test> tests
        {
            get
            {
                return _tests;
            }
            set
            {
                SetProperty(ref _tests, value);
                //PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
            }
        }

        #endregion

        #region Methods

        public async void LoadTests()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;
            //var tests = new List<Test>();
            tests = new ObservableCollection<Test>(await _TestManager.GetTestsAsync(@"C:\Visual Studio Code\StudentTestReporting\StudentTestReporting\SaveFiles\test.json"));
            //if (tests == null || tests.Count == 0)
            //{
            //    Test test = new Test();
            //    test.TestList();
            //    tests.Add(test);
            PropertyChanged(this, new PropertyChangedEventArgs("Tests"));
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnDelete()
        {
            tests.Remove(SelectedTest);
        }

        //TODO: Remove below method - its a temp method for the Add Test > Charting workflow
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

        #endregion







    }
}
