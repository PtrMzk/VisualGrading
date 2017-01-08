using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using VisualGrading.Business;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Helpers.EnumLibrary;
using VisualGrading.Presentation;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Charts
{
    public class ChartViewModel : BaseViewModel
    {
        private readonly IBusinessManager _businessManager;

        private PlotModel _gradeChart;

        private const string MAIN_FONT = "Segoe UI";

        public ChartViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();

            //todo:default to Student plotting for now
            ChartByStudents();

            NewChartCommand = new RelayCommand<string>(NewChartRequested);
            
        }


        public List<string> ComboBoxValues
        {
            get { return Enum.GetNames(typeof(ChartGrouping)).ToList(); }
        }

        public RelayCommand<string> NewChartCommand { get; private set; }

        public PlotModel GradeChart
        {
            get { return _gradeChart; }
            private set { SetProperty(ref _gradeChart, value); }
        }

        public void ChartByStudent(Student studentToFilterOn)
        {
            var studentsToFilterOn = new List<Student> {studentToFilterOn};
            ChartByStudents(studentsToFilterOn);
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

        public void ChartByStudents(List<Student> studentsToFilterOn = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsToFilterOn);
            var distinctStudents = new HashSet<string>();
            var distinctTests = new HashSet<string>();

            CreateBasePlotModel("Student Chart");

            FindDistinctStudentsAndTests(grades, distinctStudents, distinctTests);

            foreach (var test in distinctTests)
            {
                var s1 = CreateColumnSeries(test);

                foreach (var grade in grades)
                    if (grade.Test.Name == test)
                        s1.Items.Add(new ColumnItem {Value = (double) grade.PercentAverage});
                GradeChart.Series.Add(s1);
            }

            var categoryAxis = CreateCategoryAxis(distinctStudents);

            GenerateAxesAndRefreshChart(categoryAxis);
        }

        private void FindDistinctStudentsAndTests(List<Grade> grades, HashSet<string> distinctStudents,
            HashSet<string> distinctTests)
        {
            foreach (var grade in grades)
            {
                distinctStudents.Add(grade.Student.FullName);
                distinctTests.Add(grade.Test.Name);
            }
        }

        private CategoryAxis CreateCategoryAxis(HashSet<string> distinctCategories)
        {
            var categoryAxis = new CategoryAxis {Position = AxisPosition.Bottom};

            foreach (var category in distinctCategories)
                categoryAxis.Labels.Add(category);

            return categoryAxis;
        }

        public void ChartByTest(Test testToFilterOn)
        {
            var tests = new List<Test> {testToFilterOn};
            ChartByTests(tests);
        }

        public void ChartByTests(List<Test> testsToFilterOn = null)
        {
            var grades = _businessManager.GetFilteredGrades(testsToFilterOn);
            var distinctStudents = new HashSet<string>();
            var distinctTests = new HashSet<string>();

            CreateBasePlotModel("Test Chart");

            FindDistinctStudentsAndTests(grades, distinctStudents, distinctTests);

            foreach (var student in distinctStudents)
            {
                var s1 = CreateColumnSeries(student);

                foreach (var grade in grades)
                    if (grade.Student.FullName == student)
                        s1.Items.Add(new ColumnItem {Value = (double) grade.PercentAverage});
                GradeChart.Series.Add(s1);
            }

            var categoryAxis = CreateCategoryAxis(distinctTests);
            GenerateAxesAndRefreshChart(categoryAxis);
        }

        private ColumnSeries CreateColumnSeries(string title)
        {
            var s1 = new ColumnSeries
            {
                Title = title,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
            };
            return s1;
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
                LegendFont= MAIN_FONT,
                LegendFontSize = 16,
                SubtitleFontSize = 22,
                DefaultFontSize = 22,
                LegendTitleFont = MAIN_FONT
            };
        }

        private void GenerateAxesAndRefreshChart(CategoryAxis categoryAxis)
        {
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                };

            GradeChart.Axes.Add(categoryAxis);
            GradeChart.Axes.Add(valueAxis);

            GradeChart.InvalidatePlot(true);
        }
    }
}