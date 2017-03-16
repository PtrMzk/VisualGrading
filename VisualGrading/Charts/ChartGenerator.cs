#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// ChartGenerator.cs
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
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.Students;
using VisualGrading.Tests;

#endregion

namespace VisualGrading.Charts
{
    public class ChartGenerator
    {
        #region Fields

        private const string MAIN_FONT = "Segoe UI";
        private const string TEST_CHART = "Test Chart";
        private const string STUDENT_CHART = "Student Chart";
        private const string PERCENT_FORMAT = "0%";
        private const string SORTPROPERTY_ID_FORMAT = "{0} || {1}";
        private readonly IBusinessManager _businessManager;

        #endregion

        #region Constructors

        public ChartGenerator()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
        }

        #endregion

        #region Public Methods

        public PlotModel ChartByStudent(Student studentFilter)
        {
            var studentsFilter = new List<Student> {studentFilter};

            return ChartByStudents(studentsFilter);
        }

        public PlotModel ChartByStudents(List<Student> studentsFilter = null, List<Test> testsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null, string title = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsFilter, testsFilter, subjectFilter,
                subCategoryFilter);
            var averageGradeByStudent = new SortedDictionary<string, ChartHelper>();
            var distinctTests = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title != null ? title : STUDENT_CHART;

            var chart = GetBasePlotModel(plotTitle);

            foreach (var grade in grades)
            {
                var studentNameAndIDComboKey = string.Format(SORTPROPERTY_ID_FORMAT, grade.Student.FullName,
                    grade.StudentID); //name is used to sort, ID is for correct bucketing in case of duplicate names

                if (grade.Points != null)
                    if (!averageGradeByStudent.ContainsKey(studentNameAndIDComboKey))
                    {
                        averageGradeByStudent.Add(studentNameAndIDComboKey,
                            new ChartHelper(grade.Student.FullName, grade.NonNullablePoints, grade.Test.MaximumPoints));
                    }
                    else
                    {
                        averageGradeByStudent[studentNameAndIDComboKey].PointsAttained += grade.NonNullablePoints;
                        averageGradeByStudent[studentNameAndIDComboKey].PointsPossible += grade.Test.MaximumPoints;
                    }

                if (!distinctTests.ContainsKey(grade.TestID))
                    distinctTests.Add(grade.TestID, grade.Test.Name);
            }

            var categoryAxis = CreateCategoryAxis(averageGradeByStudent, columnSeries);

            AddAxesToChart(chart, categoryAxis, columnSeries);

            return chart;
        }

        public PlotModel ChartByTests(Student studentFilter)
        {
            var studentsFilter = new List<Student> {studentFilter};

            return ChartByTests(studentsFilter);
        }

        public PlotModel ChartByTests(List<Student> studentsFilter = null, List<Test> testsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null, string title = null)
        {
            var grades = _businessManager.GetFilteredGrades(studentsFilter, testsFilter, subjectFilter,
                subCategoryFilter);
            var averageGradeByTest = new SortedDictionary<string, ChartHelper>();
            var distinctStudents = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title != null ? title : TEST_CHART;

            var chart = GetBasePlotModel(plotTitle);

            foreach (var grade in grades)
            {
                var dateAndTestIDComboKey = string.Format(SORTPROPERTY_ID_FORMAT, grade.Test.Date.ToString("yy-MM-dd"),
                    grade.TestID); // we actually want tests to be sorted by date. ID is used for correct bucketing. 

                if (grade.Points != null)
                    if (!averageGradeByTest.ContainsKey(dateAndTestIDComboKey))
                    {
                        averageGradeByTest.Add(dateAndTestIDComboKey,
                            new ChartHelper(grade.Test.Name, grade.NonNullablePoints, grade.Test.MaximumPoints));
                    }
                    else
                    {
                        averageGradeByTest[dateAndTestIDComboKey].PointsAttained += grade.NonNullablePoints;
                        averageGradeByTest[dateAndTestIDComboKey].PointsPossible += grade.Test.MaximumPoints;
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
            Func<double, string> labelFormatter = percents => percents.ToString(PERCENT_FORMAT);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 1.1,
                Maximum = 1,
                LabelFormatter = labelFormatter
            };

            chart.Series.Add(columnSeries);
            chart.Axes.Add(categoryAxis);
            chart.Axes.Add(valueAxis);
        }

        private CategoryAxis CreateCategoryAxis(SortedDictionary<string, ChartHelper> averageGradeSortedDictionary,
            ColumnSeries columnSeries)
        {
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Angle = 45
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
                PlotMargins = new OxyThickness(75, double.NaN, 60, double.NaN),
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