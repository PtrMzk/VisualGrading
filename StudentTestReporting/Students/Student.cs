using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace StudentTestReporting.Students
{
    [Serializable]
    public class Student
    {
        public string Name
        {
            get;
            set;

        }
        public string Nickname { get; set; }

        #region StudentRefreshEvent
        public delegate void StudentRefreshEventHandler(object sender, EventArgs args);

        public event StudentRefreshEventHandler StudentRefreshing;


        protected virtual void OnStudentRefreshing()
        {
            if (StudentRefreshing != null)
            {
                StudentRefreshing(this, EventArgs.Empty);
            }
        }
        #endregion

        List<Student> students = new List<Student>();

        public List<Student> StudentList()
        {

            if (students.Count == 0)
            {
                students.Add(new Student()
                {
                    Name = "Piotr Mikolajczyk",
                    Nickname = "Piotr the Great"
                }
                );

                students.Add(new Student()
                {
                    Name = "Liz Lipman",
                    Nickname = "Cool Liz"
                }
                );
            }
            return students;
        }

        public List<Student> StudentList(string newName, string newNickname)
        {

            students.Add(new Student()
            {
                Name = newName,
                Nickname = newNickname
            }
            );
            OnStudentRefreshing();
            return students;
        }
    }
}
