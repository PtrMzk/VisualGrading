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
        private StudentViewModel _studentViewModel;
        private GradeViewModel _gradesViewModel = new GradeViewModel();
        private ChartViewModel _ChartViewModel = new ChartViewModel();
        private AddEditTestViewModel _addEditTestViewModel;
        private AddEditTestSeriesViewModel _addEditTestSeriesViewModel;
        private AddEditStudentViewModel _addEditStudentViewModel;

        private Presentation.BaseViewModel _currentViewModel;

        public MainWindowViewModel()
        {
            _testViewModel = ContainerHelper.Container.Resolve<TestViewModel>();
            _addEditTestViewModel = ContainerHelper.Container.Resolve<AddEditTestViewModel>();
            _addEditTestSeriesViewModel = ContainerHelper.Container.Resolve<AddEditTestSeriesViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();
            NavCommand = new RelayCommand<string>(OnNav);
            _testViewModel.AddRequested += NavToAddTest;
            _testViewModel.AddSeriesRequested += NavToAddTestSeries;
            _testViewModel.EditRequested += NavToEditTest;
            _addEditTestViewModel.Done += NavToTestList;
            _addEditTestSeriesViewModel.Done += NavToTestList;

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
                    CurrentViewModel = _studentViewModel;
                    break;
                case "grades":
                    CurrentViewModel = _gradesViewModel;
                    break;
                case "charts":
                    CurrentViewModel = _ChartViewModel;
                    break;
                case "tests":
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

        private void NavToAddTestSeries(TestSeries testSeries)
        {
            _addEditTestSeriesViewModel.SetTestSeries(testSeries);
            CurrentViewModel = _addEditTestSeriesViewModel;
        }


    }
}
