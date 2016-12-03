using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using VisualGrading;
using VisualGrading.Charts;
using VisualGrading.Grades;
using VisualGrading.Tests;
using VisualGrading.Helpers;
using VisualGrading.Students;

namespace VisualGrading
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
        private ITestManager _testManager;
        private IStudentManager _studentManager;
        private IGradeManager _gradeManager;

        private Presentation.BaseViewModel _currentViewModel;

        public MainWindowViewModel()
        {
            //ensure all managers are created when application starts
            //though they may not be used by this class at all
            _testManager = ContainerHelper.Container.Resolve<ITestManager>();
            _studentManager = ContainerHelper.Container.Resolve<IStudentManager>();
            //_gradeManager = ContainerHelper.Container.Resolve<IGradeManager>();
            _gradeManager = new GradeManager();

            _testViewModel = ContainerHelper.Container.Resolve<TestViewModel>();
            _addEditTestViewModel = ContainerHelper.Container.Resolve<AddEditTestViewModel>();
            _addEditTestSeriesViewModel = ContainerHelper.Container.Resolve<AddEditTestSeriesViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();

            NavCommand = new RelayCommand<string>(OnNav);

            _testViewModel.AddRequested += NavToAddTest;
            _testViewModel.AddSeriesRequested += NavToAddTestSeries;
            _studentViewModel.AddRequested += NavToAddStudent;
            _testViewModel.EditRequested += NavToEditTest;
            _studentViewModel.EditRequested += NavToEditStudent;
            _addEditTestViewModel.Done += NavToTestList;
            _addEditStudentViewModel.Done += NavToStudentList;
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

        private void NavToAddStudent(Student student)
        {
            //_ChartViewModel.Test = test;
            //CurrentViewModel = _ChartViewModel;
            _addEditStudentViewModel.EditMode = false;
            _addEditStudentViewModel.SetStudent(student);
            CurrentViewModel = _addEditStudentViewModel;
        }

        private void NavToEditStudent(Student student)
        {
            _addEditStudentViewModel.EditMode = true;
            _addEditStudentViewModel.SetStudent(student);
            CurrentViewModel = _addEditStudentViewModel;
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

        private void NavToStudentList()
        {
            //TODO: Hack to get students lists to refresh with changes
            _studentViewModel.LoadStudents();
            CurrentViewModel = _studentViewModel;
        }

        private void NavToAddTestSeries(TestSeries testSeries)
        {
            _addEditTestSeriesViewModel.SetTestSeries(testSeries);
            CurrentViewModel = _addEditTestSeriesViewModel;
        }


    }
}
