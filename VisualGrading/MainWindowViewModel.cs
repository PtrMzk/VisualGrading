using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.Charts;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Presentation;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private readonly AddEditStudentViewModel _addEditStudentViewModel;
        private readonly AddEditTestSeriesViewModel _addEditTestSeriesViewModel;
        private readonly AddEditTestViewModel _addEditTestViewModel;
        private readonly ChartViewModel _ChartViewModel = new ChartViewModel();

        private BaseViewModel _currentViewModel;

        private IDataManager _dataManager;
        private readonly GradeViewModel _gradeViewModel;
        private readonly StudentViewModel _studentViewModel;
        private readonly TestViewModel _testViewModel;

        public MainWindowViewModel()
        {
            //ensure all repositories are created when application starts
            //though they may not be used by this class at all

            //Mapper.Initialize(cfg => {
            //    cfg.CreateMap<VisualGrading.Model.Data.StudentDTO, VisualGrading.Students.StudentDTO>();
            //});

            //AutoMapper.Mapper.Instance.

            Mapper.Initialize(cfg => { cfg.AddProfile<AutoMapperProfile>(); });

            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();

            _testViewModel = ContainerHelper.Container.Resolve<TestViewModel>();
            _addEditTestViewModel = ContainerHelper.Container.Resolve<AddEditTestViewModel>();
            _addEditTestSeriesViewModel = ContainerHelper.Container.Resolve<AddEditTestSeriesViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();
            _gradeViewModel = ContainerHelper.Container.Resolve<GradeViewModel>();

            NavCommand = new RelayCommand<string>(OnNav);

            _testViewModel.AddRequested += NavToAddTest;
            _testViewModel.AddSeriesRequested += NavToAddTestSeries;
            _studentViewModel.AddRequested += NavToAddStudent;
            _testViewModel.EditRequested += NavToEditTest;
            _studentViewModel.EditRequested += NavToEditStudent;
            _addEditTestViewModel.Done += NavToTestList;
            _addEditStudentViewModel.Done += NavToStudentList;
            _addEditTestSeriesViewModel.Done += NavToTestList;

            //default to Grades list
            OnNav("grades");
        }

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void NavToAddTest(Test test)
        {
            //_ChartViewModel.TestDTO = test;
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
            //_ChartViewModel.TestDTO = test;
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

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "students":
                    CurrentViewModel = _studentViewModel;
                    break;
                case "grades":
                    CurrentViewModel = _gradeViewModel;
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
            //_testViewModel.LoadTests();
            CurrentViewModel = _testViewModel;
        }

        private void NavToStudentList()
        {
            //TODO: Hack to get students lists to refresh with changes
            //_studentViewModel.LoadStudents();
            CurrentViewModel = _studentViewModel;
        }

        private void NavToAddTestSeries(TestSeries testSeries)
        {
            _addEditTestSeriesViewModel.SetTestSeries(testSeries);
            CurrentViewModel = _addEditTestSeriesViewModel;
        }
    }
}