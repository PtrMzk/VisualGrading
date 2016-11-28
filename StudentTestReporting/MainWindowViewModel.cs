using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using StudentTestReporting;
using StudentTestReporting.Charts;
using StudentTestReporting.Grades;
using StudentTestReporting.Tests;
using StudentTestReporting.Helpers;
using StudentTestReporting.Students;

namespace StudentTestReporting
{
    class MainWindowViewModel : Presentation.BaseViewModel
    {
        private TestViewModel _testViewModel;
        private StudentViewModel _StudentViewModel = new StudentViewModel();
        private GradeViewModel _gradesViewModel = new GradeViewModel();
        private ChartViewModel _ChartViewModel = new ChartViewModel();
        private AddEditTestViewModel _addEditTestViewModel;

        private Presentation.BaseViewModel _currentViewModel;

        public MainWindowViewModel()
        {
            _testViewModel = ContainerHelper.Container.Resolve<TestViewModel>();
            _addEditTestViewModel = ContainerHelper.Container.Resolve<AddEditTestViewModel>();
            NavCommand = new RelayCommand<string>(OnNav);
            _testViewModel.AddTestRequested += NavToAddTest;
            _testViewModel.EditTestRequested += NavToEditTest;
            _addEditTestViewModel.Done += NavToTestList;
        }

        public Presentation.BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private void NavToAddTest(Test test)
        {
            //_ChartViewModel.Test = test;
            //CurrentViewModel = _ChartViewModel;
            _addEditTestViewModel.EditMode = false;
            _addEditTestViewModel.SetTest(test);
            CurrentViewModel = _addEditTestViewModel;
        }

        private void NavToEditTest(Test test)
        {
            _addEditTestViewModel.EditMode = true;
            _addEditTestViewModel.SetTest(test);
            CurrentViewModel = _addEditTestViewModel; 
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {

                case "students":
                    CurrentViewModel = _StudentViewModel;
                    break;
                case "grades":
                    CurrentViewModel = _gradesViewModel;
                    break;
                case "charts":
                    CurrentViewModel = _ChartViewModel;
                    break;
                case "ObservableTests":
                default:
                    CurrentViewModel = _testViewModel;
                    break;

            }
        }

        private void NavToTestList()
        {
            //TODO: Hack to get test lists to refresh with changes
            _testViewModel.LoadTests();
            CurrentViewModel = _testViewModel;
        }


    }
}
