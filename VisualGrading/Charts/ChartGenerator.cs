using System.Collections.Generic;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Charts
{
    public class ChartGenerator
    {
        #region Fields

        private const string MAIN_FONT = "Segoe UI";
        private const string TEST_CHART = "Test Chart";
        private const string STUDENT_CHART = "Student Chart";
        private readonly IBusinessManager _businessManager;

        #endregion

        #region Constructors

        public ChartGenerator()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
        }

        #endregion

        #region Public Methods

        public PlotModel ChartByStudents(List<Student> studentsFilter = null, List<Test> testsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null, string title = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsFilter, testsFilter, subjectFilter,
                subCategoryFilter);
            var averageGradeByStudent = new SortedDictionary<long, ChartHelper>();
            var distinctTests = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title != null ? title : STUDENT_CHART;

            var chart = GetBasePlotModel(plotTitle);

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

            AddAxesToChart(chart, categoryAxis, columnSeries);

            return chart;
        }

        public PlotModel ChartByTests(List<Student> studentsFilter = null, List<Test> testsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null, string title = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsFilter, testsFilter, subjectFilter,
                subCategoryFilter);
            var averageGradeByTest = new SortedDictionary<long, ChartHelper>();
            var distinctStudents = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title != null ? title : TEST_CHART;

            var chart = GetBasePlotModel(plotTitle);

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

            AddAxesToChart(chart, categoryAxis, columnSeries);
            return chart;
        }

        #endregion

        #region Private Methods

        private void AddAxesToChart(PlotModel chart, CategoryAxis categoryAxis, ColumnSeries columnSeries)
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

            chart.Series.Add(columnSeries);
            chart.Axes.Add(categoryAxis);
            chart.Axes.Add(valueAxis);
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

        private ColumnSeries CreateColumnSeries()
        {
            var series = new ColumnSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };
            return series;
        }

        private PlotModel GetBasePlotModel(string title)
        {
            var chart = new PlotModel
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

            chart.InvalidatePlot(true);

            return chart;
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