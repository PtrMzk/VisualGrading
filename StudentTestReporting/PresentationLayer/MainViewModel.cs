using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentTestReporting.Presentation
{
    using Grades;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    public class MainViewModel
    {
        public PlotModel TestModel { get; private set; }

        public MainViewModel()
        {

        }



        public void PlotGrades(List<Grade> Grades, string graphGrouping)
        {
            TestModel = new PlotModel
            {
                Title = "",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };




            HashSet<string> distinctStudentList = new HashSet<string>();
            HashSet<string> distinctTestList = new HashSet<string>();
            DistinctSubjectComparer dsComparer = new DistinctSubjectComparer();
            HashSet<DistinctSubject> distinctSubjectList = new HashSet<DistinctSubject>(dsComparer);

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom };




            int maxTestNumber = 0;
            foreach (Grade grade in Grades)
            {
                int defaultTestNumber = 1;
                distinctStudentList.Add(grade.Name);
                distinctTestList.Add(grade.Subject + "||" + grade.TestNumber.ToString());
                distinctSubjectList.Add((new DistinctSubject() { Subject = grade.Subject, maxTestNumber = defaultTestNumber }));

                if (grade.TestNumber > maxTestNumber)
                {
                    maxTestNumber = grade.TestNumber;
                }

            }


            foreach (DistinctSubject subject in distinctSubjectList)
            {
                int maxSubjectTestNumber = 0;
                {
                    foreach (Grade grade in Grades)
                    {
                        if (grade.TestNumber > maxSubjectTestNumber && grade.Subject == subject.Subject)
                        {
                            maxSubjectTestNumber = grade.TestNumber;
                        }
                    }
                    subject.maxTestNumber = maxSubjectTestNumber;
                }
            }


            if (graphGrouping == "Student")
            {
                foreach (string test in distinctTestList)
                {
                    var s1 = new ColumnSeries { Title = test.ToString().Replace("||", " "), StrokeColor = OxyColors.Black, StrokeThickness = 1 };
                    foreach (Grade grade in Grades)
                    {
                        if (grade.Subject + "||" + grade.TestNumber.ToString() == test)
                        {
                            s1.Items.Add(new ColumnItem { Value = grade.Grades });
                        }

                    }
                    TestModel.Series.Add(s1);
                }

                foreach (string Name in distinctStudentList)
                {
                    categoryAxis.Labels.Add(Name);
                }
            }

            else if (graphGrouping == "Subject")
            {
                //  foreach (string subject in distinctSubjectList)
                //  {

                var tempGrades = Grades.OrderBy(x => x.Subject).ToList();
                List<Grade> tempGradesAveraged = new List<Grade>();
                //

                //
                //
                //**




                //REWRITE TO USE THE LIST COUNT AS THE NUMBER OF GROUPS
                //THEN CYCLE THROUGH TEST NUMBERS PER SUBJECT
                //THIS WILL BE MORE LEGIBLE AND REDUCE BUGS


                //
                //insert blank ObservableTests so each subject has same number of ObservableTests
                foreach (DistinctSubject subject in distinctSubjectList)
                {
                    if (subject.maxTestNumber < maxTestNumber)
                    {
                        for (int i = subject.maxTestNumber; i < maxTestNumber; i++)
                        {
                            foreach (string student in distinctStudentList)
                            {
                                tempGrades.Add(new Grade() { Name = student, Nickname = string.Empty, Subject = subject.Subject, TestNumber = i, Grades = 0 });
                            }
                        }
                    }
                }


                foreach (DistinctSubject subject in distinctSubjectList)
                {
                    {
                        for (int i = 1; i <= maxTestNumber; i++)
                        {
                            int gradeSum = 0;
                            int avgGrade = 0;
                            foreach(Grade grade in tempGrades)
                            {
                                if(grade.Subject == subject.Subject && grade.TestNumber == i)
                                {
                                    gradeSum = gradeSum + grade.Grades;
                                }
                               
                            }
                            avgGrade = gradeSum / distinctStudentList.Count;
                            tempGradesAveraged.Add(new Grade() { Name = subject.Subject + "||" + i.ToString(), Nickname = string.Empty, Subject = subject.Subject, TestNumber = i, Grades = avgGrade });
                        }
                    }
                }



                //populate subjects up to the max number of ObservableTests

                var gradesOrderBySubject = tempGradesAveraged.OrderBy(x => x.Subject).ToList();

                for (int i = 1; i <= maxTestNumber; i++)
                {
                    var s1 = new ColumnSeries { Title = "TEST" + " " + i.ToString(), StrokeColor = OxyColors.Black, StrokeThickness = 1 };

                    foreach (Grade grade in gradesOrderBySubject)
                    {
                        if (grade.TestNumber == i)
                        {
                            s1.Items.Add(new ColumnItem { Value = grade.Grades });
                        }
                    }
                    TestModel.Series.Add(s1);
                }


                var distinctSubjectsOrdered = distinctSubjectList.OrderBy(x => x.Subject).ToList();

                foreach (DistinctSubject subject in distinctSubjectsOrdered)
                {
                    categoryAxis.Labels.Add(subject.Subject);
                }
            }




            var valueAxis = new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };


            TestModel.Axes.Add(categoryAxis);
            TestModel.Axes.Add(valueAxis);

            TestModel.InvalidatePlot(true);
        }



    }

    internal sealed class DistinctSubject
    {
        public string Subject;
        public int maxTestNumber;
    }

    internal sealed class DistinctSubjectComparer : IEqualityComparer<DistinctSubject>
    {
        public bool Equals(DistinctSubject i1, DistinctSubject i2)
        {
            bool rslt = ((i1.Subject == i2.Subject) && (i1.maxTestNumber == i2.maxTestNumber));
            return rslt;
        }

        public int GetHashCode(DistinctSubject x)
        {
            return x.Subject.GetHashCode() ^ x.maxTestNumber.GetHashCode();
        }
    }

}

