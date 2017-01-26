using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Wpf;
using VisualGrading.Helpers;
using VisualGrading.Helpers.EnumLibrary;
using VisualGrading.Presentation;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Charts
{
    public class ChartViewModel : BaseViewModel
    {
        #region Fields

        private readonly IFileDialog _fileDialog;

        private readonly ChartGenerator _chartGenerator;

        private PlotModel _gradeChart;

        #endregion

        #region Constructors

        public ChartViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _fileDialog = ContainerHelper.Container.Resolve<IFileDialog>();

            _chartGenerator = new ChartGenerator();

            //todo:default to Student plotting for now
            GradeChart = _chartGenerator.ChartByStudents();

            ExportCommand = new RelayCommand(OnExport);
            NewChartCommand = new RelayCommand<string>(NewChartRequested);
        }

        #endregion

        #region Properties

        public List<string> ComboBoxValues
        {
            get { return Enum.GetNames(typeof(ChartGrouping)).ToList(); }
        }

        public RelayCommand<string> NewChartCommand { get; private set; }
        public RelayCommand ExportCommand { get; private set; }

        public PlotModel GradeChart
        {
            get { return _gradeChart; }
            private set { SetProperty(ref _gradeChart, value); }
        }

        #endregion

        #region Public Methods

        public void ChartByStudent(Student studentFilter)
        {
            var studentsFilter = new List<Student> {studentFilter};
            GradeChart = _chartGenerator.ChartByStudents(studentsFilter, null, null, null, studentFilter.FullName);
        }

        public void ChartByTest(Test testFilter)
        {
            var tests = new List<Test> {testFilter};
            GradeChart = _chartGenerator.ChartByTests(null, tests, null, null, testFilter.Name);
        }

        public void ChartStudentsBySubCategory(string subCategory)
        {
            GradeChart = _chartGenerator.ChartByStudents(null, null, null, subCategory, subCategory);
        }

        public void ChartStudentsBySubject(string subject)
        {
            GradeChart = _chartGenerator.ChartByStudents(null, null, subject, null, subject);
        }

        public void ChartStudentsByTest(Test testFilter)
        {
            var testsFilter = new List<Test> {testFilter};
            GradeChart = _chartGenerator.ChartByStudents(null, testsFilter, null, null, testFilter.Name);
        }

        public void ChartTestsByStudent(Student studentFilter)
        {
            var studentsFilter = new List<Student> {studentFilter};
            GradeChart = _chartGenerator.ChartByTests(studentsFilter, null, null, null, studentFilter.FullName);
        }

        public void ChartTestsBySubCategory(string subCategory)
        {
            GradeChart = _chartGenerator.ChartByTests(null, null, null, subCategory, subCategory);
        }

        public void ChartTestsBySubject(string subject)
        {
            GradeChart = _chartGenerator.ChartByTests(null, null, subject, null, subject);
        }

        #endregion

        #region Private Methods

        private void NewChartRequested(string grouping)
        {
            ChartGrouping chartGrouping;
            Enum.TryParse(grouping, out chartGrouping);
            switch (chartGrouping)
            {
                case ChartGrouping.Student:
                    GradeChart = _chartGenerator.ChartByStudents();
                    return;
                case ChartGrouping.Test:
                default:
                    GradeChart = _chartGenerator.ChartByTests();
                    return;
            }
        }

        private void OnExport()
        {
            _fileDialog.SaveFileDialog.Filter = "PNG|*.png";
            _fileDialog.SaveFileDialog.Title = GradeChart.Title;

            if (_fileDialog.SaveFileDialog.ShowDialog() == true)
            {
                var pngExporter = new PngExporter {Width = 1920, Height = 1080, Background = OxyColors.White};

                pngExporter.ExportToFile(GradeChart, _fileDialog.SaveFileDialog.FileName);
            }
        }

        #endregion
    }
}