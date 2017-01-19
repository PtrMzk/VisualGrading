using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.Helpers.EnumLibrary;
using VisualGrading.Students;
using VisualGrading.Tests;
using VisualGrading.ViewModelHelpers;
using CategoryAxis = OxyPlot.Axes.CategoryAxis;
using ColumnSeries = OxyPlot.Series.ColumnSeries;
using LinearAxis = OxyPlot.Axes.LinearAxis;

namespace VisualGrading.Charts
{
    public class ChartViewModel : BaseViewModel
    {
        #region Fields

        private const string MAIN_FONT = "Segoe UI";
        private const string TEST_CHART = "Test Chart";
        private const string STUDENT_CHART = "Student Chart";
        private readonly IBusinessManager _businessManager;
        private readonly IFileDialog _fileDialog;

        private PlotModel _gradeChart;

        #endregion

        #region Constructors

        public ChartViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();

            _fileDialog = ContainerHelper.Container.Resolve<IFileDialog>();

            //todo:default to Student plotting for now
            ChartByStudents();

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

        #region Methods

        public void ChartByStudent(Student studentFilter)
        {
            var studentsFilter = new List<Student> {studentFilter};
            ChartByStudents(studentsFilter, null, null, null, studentFilter.FullName);
        }

        public void ChartStudentsByTest(Test testFilter)
        {
            var testsFilter = new List<Test> {testFilter};
            ChartByStudents(null, testsFilter, null, null, testFilter.Name);
        }

        public void ChartStudentsBySubject(string subject)
        {
            ChartByStudents(null, null, subject, null, subject);
        }

        public void ChartStudentsBySubCategory(string subCategory)
        {
            ChartByStudents(null, null, null, subCategory, subCategory);
        }

        public void ChartByStudents(List<Student> studentsFilter = null, List<Test> testsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null, string title = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsFilter, testsFilter, subjectFilter,
                subCategoryFilter);
            var averageGradeByStudent = new SortedDictionary<long, ChartHelper>();
            var distinctTests = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title != null ? title : STUDENT_CHART;

            CreateBasePlotModel(plotTitle);

            foreach (var grade in grades)
            {
                if (!averageGradeByStudent.ContainsKey(grade.StudentID))
                {
                    averageGradeByStudent.Add(grade.StudentID,
                        new ChartHelper(grade.Student.FullName, grade.Points, grade.Test.MaximumPoints));
                }
                else
                {
                    averageGradeByStudent[grade.StudentID].PointsAttained += grade.Points;
                    averageGradeByStudent[grade.StudentID].PointsPossible += grade.Test.MaximumPoints;
                }

                if (!distinctTests.ContainsKey(grade.TestID))
                    distinctTests.Add(grade.TestID, grade.Test.Name);
            }

            var categoryAxis = CreateCategoryAxis(averageGradeByStudent, columnSeries);

            GenerateAxesAndRefreshChart(categoryAxis, columnSeries);
        }

        public void ChartByTest(Test testFilter)
        {
            var tests = new List<Test> {testFilter};
            ChartByTests(null, tests, null, null, testFilter.Name);
        }

        public void ChartTestsByStudent(Student studentFilter)
        {
            var studentsFilter = new List<Student> {studentFilter};
            ChartByTests(studentsFilter, null, null, null, studentFilter.FullName);
        }

        public void ChartTestsBySubject(string subject)
        {
            ChartByTests(null, null, subject, null, subject);
        }

        public void ChartTestsBySubCategory(string subCategory)
        {
            ChartByTests(null, null, null, subCategory, subCategory);
        }

        public void ChartByTests(List<Student> studentsFilter = null, List<Test> testsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null, string title = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsFilter, testsFilter, subjectFilter,
                subCategoryFilter);
            var averageGradeByTest = new SortedDictionary<long, ChartHelper>();
            var distinctStudents = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title != null ? title : TEST_CHART;

            CreateBasePlotModel(plotTitle);

            foreach (var grade in grades)
            {
                if (!averageGradeByTest.ContainsKey(grade.TestID))
                {
                    averageGradeByTest.Add(grade.TestID,
                        new ChartHelper(grade.Test.Name, grade.Points, grade.Test.MaximumPoints));
                }
                else
                {
                    averageGradeByTest[grade.TestID].PointsAttained += grade.Points;
                    averageGradeByTest[grade.TestID].PointsPossible += grade.Test.MaximumPoints;
                }

                if (!distinctStudents.ContainsKey(grade.TestID))
                    distinctStudents.Add(grade.TestID, grade.Student.FullName);
            }

            var categoryAxis = CreateCategoryAxis(averageGradeByTest, columnSeries);

            GenerateAxesAndRefreshChart(categoryAxis, columnSeries);
        }

        private CategoryAxis CreateCategoryAxis(SortedDictionary<long, ChartHelper> averageGradeSortedDictionary,
            ColumnSeries columnSeries)
        {
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom
            };

            foreach (var grouping in averageGradeSortedDictionary)
            {
                categoryAxis.Labels.Add(grouping.Value.Name);

                columnSeries.Items.Add(new ColumnItem {Value = (double) grouping.Value.PointsAverage});
            }
            return categoryAxis;
        }

        private void NewChartRequested(string grouping)
        {
            ChartGrouping chartGrouping;
            Enum.TryParse(grouping, out chartGrouping);
            switch (chartGrouping)
            {
                case ChartGrouping.Student:
                    ChartByStudents();
                    return;
                case ChartGrouping.Test:
                default:
                    ChartByTests();
                    return;
            }
        }

        private ColumnSeries CreateColumnSeries()
        {
            var series = new ColumnSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };
            return series;
        }

        private void CreateBasePlotModel(string title)
        {
            GradeChart = new PlotModel
            {
                Title = title,
                DefaultFont = MAIN_FONT,
                TitleFont = MAIN_FONT,
                TitleFontWeight = 1,
                TitleFontSize = 32,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                PlotAreaBackground = OxyColor.Parse("#FFFFFFFF"),
                Background = ColorHelper.OxyBackgroundColor,
                DefaultColors = ColorHelper.OxyColorPalete,
                LegendFont = MAIN_FONT,
                LegendFontSize = 16,
                SubtitleFontSize = 22,
                DefaultFontSize = 22,
                LegendTitleFont = MAIN_FONT
            };
        }

        private void GenerateAxesAndRefreshChart(CategoryAxis categoryAxis, ColumnSeries columnSeries)
        {
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 1.1,
                Maximum = 1
            };

            GradeChart.Series.Add(columnSeries);
            GradeChart.Axes.Add(categoryAxis);
            GradeChart.Axes.Add(valueAxis);

            GradeChart.InvalidatePlot(true);
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

    internal class ChartHelper
    {
        #region Fields

        public string Name;
        public decimal PointsAttained;
        public decimal PointsPossible;

        #endregion

        #region Constructors

        public ChartHelper(string name, decimal pointsAttained, decimal pointsPossible)
        {
            Name = name;
            PointsAttained = pointsAttained;
            PointsPossible = pointsPossible;
        }

        #endregion

        #region Properties

        public decimal PointsAverage => PointsAttained / PointsPossible;

        #endregion
    }
}