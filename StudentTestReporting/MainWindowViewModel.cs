using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting;
using StudentTestReporting.Charts;
using StudentTestReporting.Grades;
using StudentTestReporting.Tests;
using StudentTestReporting.Helpers;
using StudentTestReporting.Students;

namespace StudentTestReporting
{
    class MainWindowViewModel : GraphicFrontEnd.BaseViewModel
    {
        private TestViewModel _testViewModel = new TestViewModel();
        private StudentViewModel _StudentViewModel = new StudentViewModel();
        private GradeViewModel _gradesViewModel = new GradeViewModel();
        private ChartViewModel _ChartViewModel = new ChartViewModel();
        private AddEditTestViewModel _addEditTestViewModel = new AddEditTestViewModel();

        private GraphicFrontEnd.BaseViewModel _currentViewModel;

        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            _testViewModel.AddTestRequested += NavToAddTest;
            _testViewModel.EditTestRequested += NavToEditTest;
        }

        public GraphicFrontEnd.BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        private void NavToAddTest(Test test)
        {
            //_ChartViewModel.Test = test;
            //CurrentViewModel = _ChartViewModel;
            _addEditTestViewModel.Mode = AddEditTestViewModel.AddEditMode.AddMode;
            _addEditTestViewModel.SetTest(test);
            CurrentViewModel = _addEditTestViewModel;
        }

        private void NavToEditTest(Test test)
        {
            _addEditTestViewModel.Mode = AddEditTestViewModel.AddEditMode.EditMode;
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
                case "tests":
                default:
                    CurrentViewModel = _testViewModel;
                    break;

            }
        }


    }
}
