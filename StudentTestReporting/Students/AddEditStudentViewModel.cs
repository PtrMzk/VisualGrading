using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Presentation;

namespace StudentTestReporting.Students
{
    public class AddEditStudentViewModel : BaseViewModel
    {
        #region Constructor

        public AddEditStudentViewModel(IStudentManager manager)
        {
            _manager = manager;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        private readonly IStudentManager _manager;

        private Student _editingStudent;

        public SimpleEditableStudent Student
        {
            get { return _Student; }
            set { SetProperty(ref _Student, value); }
        }

        private SimpleEditableStudent _Student;

        private bool _editMode;

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

        public event Action Done = delegate { };

        #endregion

        #region Methods

        public void SetStudent(Student Student)
        {
            _editingStudent = Student;
            if (Student != null) Student.ErrorsChanged -= RaiseCanExecuteChanged;
            Student = new SimpleEditableStudent();
            Student.ErrorsChanged += RaiseCanExecuteChanged;
            CopyStudent(Student, Student);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void UpdateStudent(SimpleEditableStudent source, Student destination)
        {
            destination.Subject = source.Subject;
            destination.StudentID = source.StudentID;
            destination.SeriesNumber = source.SeriesNumber;
            destination.Date = source.Date;
            destination.Name = source.Name;
            destination.SubCategory = source.SubCategory;
        }

        private void CopyStudent(Student source, SimpleEditableStudent destination)
        {
            destination.StudentID = source.StudentID;

            if (EditMode)
            {
                destination.Subject = source.Subject;
                destination.SeriesNumber = source.SeriesNumber;
                destination.Date = source.Date;
                destination.Name = source.Name;
                destination.SubCategory = source.SubCategory;
            }
        }

        private bool CanSave()
        {
            return !Student.HasErrors;
        }

        private async void OnSave()
        {
            UpdateStudent(Student, _editingStudent);
            if (EditMode)
                _manager.UpdateStudentAsync(_editingStudent);
            else
                _manager.AddStudentAsync(_editingStudent);
            Done();
        }

        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}
