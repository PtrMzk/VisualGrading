using VisualGrading.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualGrading.Grades
{
    public class GradeViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Grade> grades{ get; set; }

        public GradeViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            //make async later

            grades = new ObservableCollection<Grade>(GradeManager.grades);
        }
    }
}
