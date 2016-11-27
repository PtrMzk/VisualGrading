using StudentTestReporting.Presentation;
using StudentTestReporting.Students;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTestReporting.Students
{
    class StudentViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Student> students { get; set; }

        public StudentViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            //make async later

            students = new ObservableCollection<Student>(StudentManager.students);
        }
    }
}
