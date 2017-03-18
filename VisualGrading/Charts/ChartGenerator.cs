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
using System.Linq;
using Microsoft.Practices.Unity;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using VisualGrading.Business;
using VisualGrading.Grades;
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
            var averageGradeByStudent = new SortedDictionary<long, ChartHelper>();
            var distinctTests = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title ?? STUDENT_CHART;

            var chart = GetBasePlotModel(plotTitle);

            foreach (var grade in grades)
            {
                var studentID = grade.StudentID; 

                if (grade.Points != null)
                    if (!averageGradeByStudent.ContainsKey(studentID))
                    {
                        averageGradeByStudent.Add(studentID,
                            new ChartHelper(grade.Student.FullName, grade));
                    }
                    else
                    {
                        averageGradeByStudent[studentID].AddGrade(grade);
                    }

                if (!distinctTests.ContainsKey(grade.TestID))
                    distinctTests.Add(grade.TestID, grade.Test.Name);
            }

            var sortedAveragedGrades = averageGradeByStudent.Values.OrderBy(x => x.Grades[0].Student.LastName).ToList();

            var categoryAxis = CreateCategoryAxis(sortedAveragedGrades, columnSeries);

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
            var averageGradeByTest = new SortedDictionary<long, ChartHelper>();
            var distinctStudents = new SortedDictionary<long, string>();
            var columnSeries = CreateColumnSeries();
            var plotTitle = title ?? TEST_CHART;

            var chart = GetBasePlotModel(plotTitle);

            foreach (var grade in grades)
            {
                var testID = grade.TestID; 

                if (grade.Points != null)
                    if (!averageGradeByTest.ContainsKey(testID))
                    {
                        averageGradeByTest.Add(testID,
                            new ChartHelper(grade.Test.Name, grade));
                    }
                    else
                    {
                        averageGradeByTest[testID].AddGrade(grade);
                    }

                if (!distinctStudents.ContainsKey(grade.TestID))
                    distinctStudents.Add(grade.TestID, grade.Student.FullName);
            }

            var sortedAveragedGrades = averageGradeByTest.Values.OrderBy(x => x.Grades[0].Test.Date).ToList();

            var categoryAxis = CreateCategoryAxis(sortedAveragedGrades, columnSeries);

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

        private CategoryAxis CreateCategoryAxis(List<ChartHelper> averagedGrades,
            ColumnSeries columnSeries)
        {
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Angle = 45
            };

            foreach (var grouping in averagedGrades)
            {
                categoryAxis.Labels.Add(grouping.Name);

                columnSeries.Items.Add(new ColumnItem {Value = (double) grouping.PointsAverage});
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

        private string _name;
        private decimal _pointsAttained;
        private decimal _pointsPossible;
        private List<Grade> _grades;

        #endregion

        #region Constructors

        public ChartHelper(string name, Grade grade)
        {
            _grades = new List<Grade>();
            _name = name;
            AddGrade(grade);
        }

        public void AddGrade(Grade grade)
        {
            _grades.Add(grade);
            _pointsAttained += grade.NonNullablePoints;
            _pointsPossible += grade.Test.MaximumPoints;
        }


        #endregion

        #region Properties

        public List<Grade> Grades
        {
            get { return _grades; }
        }

        public string Name
        {
            get { return _name;}
        }

        public decimal PointsAverage => _pointsAttained / _pointsPossible;

        #endregion
    }
}