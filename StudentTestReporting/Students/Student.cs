using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace VisualGrading.Students
{
    [Serializable]
    public class Student
    {
        public Student()
        {
            this.StudentID = Guid.NewGuid();
        }

        public Guid StudentID { get; private set; }
        public string FirstName {get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", !string.IsNullOrEmpty(Nickname) ? Nickname : FirstName, LastName); }} 
        public string Nickname { get; set; }
        public string EmailAddress { get; set; }
        public string ParentEmailAddress { get; set; }
        public decimal OverallGrade { get; set; }

        #region StudentRefreshEvent
        //TODO: is this event needed? 
        //public delegate void StudentRefreshEventHandler(object sender, EventArgs args);

        //public event StudentRefreshEventHandler StudentRefreshing;

        //protected virtual void OnStudentRefreshing()
        //{
        //    if (StudentRefreshing != null)
        //    {
        //        StudentRefreshing(this, EventArgs.Empty);
        //    }
        //}
        #endregion

      }
}
