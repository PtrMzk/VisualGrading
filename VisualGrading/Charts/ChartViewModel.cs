using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        

        public ChartViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();

            //todo:default to Student plotting for now
            ChartByStudents();

            NewChartCommand = new RelayCommand<string>(NewChartRequested);

        }

        public List<string> ComboBoxValues { get { return Enum.GetNames(typeof(ChartGrouping)).ToList(); } }

        public RelayCommand<string> NewChartCommand { get; private set; }

        private PlotModel _gradeChart;

        public PlotModel GradeChart
        {
            get { return _gradeChart; }
            private set
            {
                SetProperty(ref _gradeChart, value);             
            }
        }
        
        public void ChartByStudent(Student studentToFilterOn)
        {
            var studentsToFilterOn = new List<Student>() { studentToFilterOn };
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

            GradeChart = new PlotModel
            {
                Title = "Student Plot",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
            };

            var distinctStudentList = new HashSet<string>();
            var distinctTestList = new HashSet<string>();


            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };


            foreach (var grade in grades)
            {
                distinctStudentList.Add(grade.Student.FullName);
                distinctTestList.Add(grade.Test.Name);
            }

            {
                foreach (var test in distinctTestList)
                {
                    var s1 = new ColumnSeries
                    {
                        Title = test,
                        StrokeColor = OxyColors.Black,
                        StrokeThickness = 1
                    };

                    foreach (var grade in grades)
                        if (grade.Test.Name == test)
                            s1.Items.Add(new ColumnItem { Value = grade.Points });
                    GradeChart.Series.Add(s1);
                }

                foreach (var Name in distinctStudentList)
                    categoryAxis.Labels.Add(Name);
            }

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0
            };


            GradeChart.Axes.Add(categoryAxis);
            GradeChart.Axes.Add(valueAxis);

            GradeChart.InvalidatePlot(true);
        }

        public void ChartByTest(Test testToFilterOn)
        {
            var tests = new List<Test>() { testToFilterOn };
            ChartByTests(tests);
        }

        public void ChartByTests(List<Test> testsToFilterOn = null)
        {
            var grades = _businessManager.GetFilteredGrades(testsToFilterOn);

            GradeChart = new PlotModel
            {
                Title = "Test Plot",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
            };

            var distinctStudentList = new HashSet<string>();
            var distinctTestList = new HashSet<string>();


            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };


            foreach (var grade in grades)
            {
                distinctStudentList.Add(grade.Student.FullName);
                distinctTestList.Add(grade.Test.Name);
            }

            {
                foreach (var student in distinctStudentList)
                {
                    var s1 = new ColumnSeries
                    {
                        Title = student,
                        StrokeColor = OxyColors.Black,
                        StrokeThickness = 1
                    };

                    foreach (var grade in grades)
                        if (grade.Student.FullName == student)
                            s1.Items.Add(new ColumnItem { Value = grade.Points });
                    GradeChart.Series.Add(s1);
                }

                foreach (var Name in distinctTestList)
                    categoryAxis.Labels.Add(Name);
            }

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0
            };


            GradeChart.Axes.Add(categoryAxis);
            GradeChart.Axes.Add(valueAxis);

            GradeChart.InvalidatePlot(true);
        }

        private PlotModel TestPlot()
        {
            var model = new PlotModel
            {
                Title = "BarSeries",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            var s1 = new BarSeries() { Title = "Series 1", StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s1.Items.Add(new BarItem { Value = 25 });
            s1.Items.Add(new BarItem { Value = 137 });
            s1.Items.Add(new BarItem { Value = 18 });
            s1.Items.Add(new BarItem { Value = 40 });

            var s2 = new BarSeries { Title = "Series 2", StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            s2.Items.Add(new BarItem { Value = 12 });
            s2.Items.Add(new BarItem { Value = 14 });
            s2.Items.Add(new BarItem { Value = 120 });
            s2.Items.Add(new BarItem { Value = 26 });

            CategoryAxis categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("Category A");
            categoryAxis.Labels.Add("Category B");
            categoryAxis.Labels.Add("Category C");
            categoryAxis.Labels.Add("Category D");
            LinearAxis valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                MaximumPadding = 0.06,
                AbsoluteMinimum = 0
            };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}