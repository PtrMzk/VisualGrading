using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Tests;
using StudentTestReporting.Grades;
using StudentTestReporting.Helpers;
using StudentTestReporting.Presentation;

namespace StudentTestReporting.Charts
{
    public class ChartViewModel : StudentTestReporting.Presentation.BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Grade> Charts { get; set; }

        public ChartViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            //make async later

            Charts = new ObservableCollection<Grade>(GradeManager.grades);
        }

        //temp code for test -> charts testing
        private Test _Test;

        public Test Test
        {
            get { return _Test; }
            set { SetProperty(ref _Test, value); }
        }

        //end temp code
    }
}

