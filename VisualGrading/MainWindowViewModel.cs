using System;
using System.IO;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.Charts;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Helpers.EnumLibrary;
using VisualGrading.Presentation;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Constructors

        public MainWindowViewModel()
        {
            InitializeVisualGrading();
        }

        #endregion

        #region Fields

        private AddEditStudentViewModel _addEditStudentViewModel;
        private AddEditTestSeriesViewModel _addEditTestSeriesViewModel;
        private AddEditTestViewModel _addEditTestViewModel;
        private ChartViewModel _chartViewModel;

        private NavigationTarget _currentTab;

        private BaseViewModel _currentViewModel;

        private IDataManager _dataManager;
        private GradeViewModel _gradeViewModel;
        private SettingsViewModel _settingsViewModel;
        private StudentViewModel _studentViewModel;
        private TestViewModel _testViewModel;

        #endregion

        #region Properties

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public NavigationTarget CurrentTab
        {
            get => _currentTab;
            set => SetProperty(ref _currentTab, value);
        }

        public RelayCommand<NavigationTarget> NavCommand { get; private set; }

        #endregion

        #region Methods

        private void InitializeVisualGrading()
        {
            //set AppData to be location of DB file
            AppDomain.CurrentDomain.SetData("DataDirectory",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "VisualGrading"));

            Mapper.Initialize(cfg => { cfg.AddProfile<AutoMapperProfile>(); });

            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();

            _testViewModel = ContainerHelper.Container.Resolve<TestViewModel>();
            _addEditTestViewModel = ContainerHelper.Container.Resolve<AddEditTestViewModel>();
            _addEditTestSeriesViewModel = ContainerHelper.Container.Resolve<AddEditTestSeriesViewModel>();
            _studentViewModel = ContainerHelper.Container.Resolve<StudentViewModel>();
            _addEditStudentViewModel = ContainerHelper.Container.Resolve<AddEditStudentViewModel>();
            _gradeViewModel = ContainerHelper.Container.Resolve<GradeViewModel>();
            _chartViewModel = ContainerHelper.Container.Resolve<ChartViewModel>();
            _settingsViewModel = ContainerHelper.Container.Resolve<SettingsViewModel>();

            NavCommand = new RelayCommand<NavigationTarget>(OnNav);

            _testViewModel.AddRequested += NavToAddTest;
            _testViewModel.AddSeriesRequested += NavToAddTestSeries;
            _studentViewModel.AddRequested += NavToAddStudent;
            _testViewModel.EditRequested += NavToEditTest;
            _studentViewModel.EditRequested += NavToEditStudent;
            _addEditTestViewModel.Done += NavToTestList;
            _addEditStudentViewModel.Done += NavToStudentList;
            _addEditTestSeriesViewModel.Done += NavToTestList;
            _testViewModel.TestChartRequested += NavToChart;
            _testViewModel.SubjectChartRequested += NavToStudentChartBySubject;
            _testViewModel.SubCategoryChartRequested += NavToStudentChartBySubCategory;

            _studentViewModel.ChartRequested += NavToChart;

            SetDefaultScreen();
        }

        private void SetDefaultScreen()
        {
            OnNav(NavigationTarget.Test);
        }

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

        private void NavToChart(Test test)
        {
            _chartViewModel.ChartStudentsByTest(test);
            CurrentViewModel = _chartViewModel;
        }

        private void NavToStudentChartBySubject(string subject)
        {
            _chartViewModel.ChartStudentsBySubject(subject);
            CurrentViewModel = _chartViewModel;
        }

        private void NavToStudentChartBySubCategory(string subCategory)
        {
            _chartViewModel.ChartStudentsBySubCategory(subCategory);
            CurrentViewModel = _chartViewModel;
        }

        private void NavToChart(Student student)
        {
            _chartViewModel.ChartTestsByStudent(student);
            CurrentViewModel = _chartViewModel;
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

        private void OnNav(NavigationTarget target)
        {
            switch (target)
            {
                case NavigationTarget.Student:
                    CurrentViewModel = _studentViewModel;
                    break;
                case NavigationTarget.Grade:
                    CurrentViewModel = _gradeViewModel;
                    break;
                case NavigationTarget.Chart:
                    CurrentViewModel = _chartViewModel;
                    break;
                case NavigationTarget.Test:
                    CurrentViewModel = _testViewModel;
                    break;
                case NavigationTarget.Settings:
                    CurrentViewModel = _settingsViewModel;
                    break;
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

        #endregion
    }
}