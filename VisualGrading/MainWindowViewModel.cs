#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// MainWindowViewModel.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

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

#endregion

namespace VisualGrading
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        private const string TESTID_SEARCH_STRING = "TestID:[{0}]";
        private const string STUDENTID_SEARCH_STRING = "StudentID:[{0}]";
        private const string ID_SEARCH_STRING = "ID:[{0}]";

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

        #region Constructors

        public MainWindowViewModel()
        {
            InitializeVisualGrading();
        }

        #endregion

        #region Properties

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public NavigationTarget CurrentTab
        {
            get { return _currentTab; }
            set { SetProperty(ref _currentTab, value); }
        }

        public RelayCommand<NavigationTarget> NavCommand { get; private set; }

        #endregion

        #region Private Methods

        private void InitializeObjects()
        {
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
        }

        private void InitializeVisualGrading()
        {
            SetDataBaseLocation();

            InitializeObjects();

            SetEventListeners();

            SetDefaultScreen();
        }

        private void NavToAddStudent(Student student)
        {
            //_ChartViewModel.TestDTO = test;
            //CurrentViewModel = _ChartViewModel;
            _addEditStudentViewModel.EditMode = false;
            _addEditStudentViewModel.SetStudent(student);
            CurrentViewModel = _addEditStudentViewModel;
        }

        private void NavToAddTest(Test test)
        {
            //_ChartViewModel.TestDTO = test;
            //CurrentViewModel = _ChartViewModel;
            _addEditTestViewModel.EditMode = false;
            _addEditTestViewModel.SetTest(test);
            CurrentViewModel = _addEditTestViewModel;
        }

        private void NavToAddTestSeries(TestSeries testSeries)
        {
            _addEditTestSeriesViewModel.SetTestSeries(testSeries);
            CurrentViewModel = _addEditTestSeriesViewModel;
        }

        private void NavToChart(Test test)
        {
            _chartViewModel.ChartStudentsByTest(test);
            OnNav(NavigationTarget.Chart);
        }

        private void NavToChart(Student student)
        {
            _chartViewModel.ChartTestsByStudent(student);
            OnNav(NavigationTarget.Chart);
        }

        private void NavToEditStudent(Student student)
        {
            _addEditStudentViewModel.EditMode = true;
            _addEditStudentViewModel.SetStudent(student);
            CurrentViewModel = _addEditStudentViewModel;
        }

        private void NavToEditTest(Test test)
        {
            _addEditTestViewModel.EditMode = true;
            _addEditTestViewModel.SetTest(test);
            CurrentViewModel = _addEditTestViewModel;
        }

        private void NavToStudent(long id)
        {
            OnNav(NavigationTarget.Student);
            var idFilter = string.Format(ID_SEARCH_STRING, id);
            _studentViewModel.SearchStudents(idFilter);
        }

        private void NavToStudentChartBySubCategory(string subCategory)
        {
            _chartViewModel.ChartStudentsBySubCategory(subCategory);
            OnNav(NavigationTarget.Chart);
        }

        private void NavToStudentChartBySubject(string subject)
        {
            _chartViewModel.ChartStudentsBySubject(subject);
            OnNav(NavigationTarget.Chart);
        }

        private void NavToStudentGrades(long id)
        {
            OnNav(NavigationTarget.Grade);
            var idFilter = string.Format(STUDENTID_SEARCH_STRING, id);
            _gradeViewModel.SearchGrades(idFilter);
        }

        private void NavToStudentList()
        {
            _studentViewModel.ClearSearch();
            OnNav(NavigationTarget.Student);
        }

        private void NavToTest(long id)
        {
            OnNav(NavigationTarget.Test);
            var idFilter = string.Format(ID_SEARCH_STRING, id);
            _testViewModel.SearchTests(idFilter);
        }

        private void NavToTestGrades(long id)
        {
            OnNav(NavigationTarget.Grade);
            var idFilter = string.Format(TESTID_SEARCH_STRING, id);
            _gradeViewModel.SearchGrades(idFilter);
        }

        private void NavToTestList()
        {
            _testViewModel.ClearSearch();
            OnNav(NavigationTarget.Test);
        }

        private void OnNav(NavigationTarget target)
        {
            CurrentTab = target;

            switch (target)
            {
                case NavigationTarget.Student:
                    _studentViewModel.LoadStudents();
                    CurrentViewModel = _studentViewModel;
                    break;
                case NavigationTarget.Grade:
                    _gradeViewModel.LoadGrades();
                    CurrentViewModel = _gradeViewModel;
                    break;
                case NavigationTarget.Chart:
                    CurrentViewModel = _chartViewModel;
                    break;
                case NavigationTarget.Test:
                    _testViewModel.LoadTests();
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

        private void SetDataBaseLocation()
        {
            //set AppData to be location of DB file
            AppDomain.CurrentDomain.SetData("DataDirectory",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "VisualGrading"));
        }

        private void SetDefaultScreen()
        {
            NavCommand = new RelayCommand<NavigationTarget>(OnNav);

            OnNav(NavigationTarget.Test);
        }

        private void SetEventListeners()
        {
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
            _testViewModel.GoToTestGradesRequested += NavToTestGrades;
            _studentViewModel.GoToStudentGradesRequested += NavToStudentGrades;
            _gradeViewModel.GoToStudentRequested += NavToStudent;
            _gradeViewModel.GoToTestRequested += NavToTest;
            _studentViewModel.ChartRequested += NavToChart;
        }

        #endregion
    }
}