using StudentTestReporting.Presentation;
using StudentTestReporting.Students;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTestReporting.Helpers;

namespace StudentTestReporting.Students
{
    public class StudentViewModel : BaseViewModel
    {
        #region Constructor

        public StudentViewModel(IStudentManager manager)
        {
            _manager = manager;
            DeleteCommand = new RelayCommand<Student>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddStudent);
            EditCommand = new RelayCommand<Student>(OnEditStudent);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            DeleteRequested += RemoveStudentFromPresentationAndManager;

            //TODO: delete the below if not needed, clean this up to set the private variable Student
            //var _observableStudents = new ObservableCollection<Student>();
            //this._observableStudents = new ObservableCollection<Student>();

            //this._observableStudents.AddStudent(new Student()
            //{
            //    Subject = "Computer Programming",
            //    StudentNumber = 1
            //});
            //this._observableStudents.AddStudent(new Student()
            //{

            //    Subject = "Computer Programming",
            //    StudentNumber = 2
            //});
            //this._observableStudents.AddStudent(new Student()
            //{

            //    Subject = "Science",
            //    StudentNumber = 1
            //});
            //PropertyChanged(this, new PropertyChangedEventArgs("Students"));

        }

        #endregion

        #region Properties

        private IStudentManager _manager;

        private ObservableCollection<Student> _observableStudents;

        public ObservableCollection<Student> ObservableStudents
        {
            get
            {
                return _observableStudents;
            }
            set
            {
                SetProperty(ref _observableStudents, value);
                //PropertyChanged(this, new PropertyChangedEventArgs("Students"));
            }
        }

        private List<Student> _allStudents;

        private Student _selectedStudent { get; set; }

        public Student SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    DeleteCommand.RaiseCanExecuteChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
                }
            }
        }

        public RelayCommand<Student> DeleteCommand { get; private set; }

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Student> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event Action<Student> AddRequested = delegate { };
        public event Action<Student> EditRequested = delegate { };
        public event Action<Student> DeleteRequested = delegate { };

        private string _searchInput;

        public string SearchInput
        {
            get
            {
                return _searchInput;

            }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterStudents(_searchInput);

            }
        }


        #endregion

        #region Methods

        public async void LoadStudents()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;
            //var ObservableStudents = new List<Student>();
            _allStudents =
                await
                    _manager.GetStudentsAsync();
            ObservableStudents = new ObservableCollection<Student>(_allStudents);
            //if (ObservableStudents == null || ObservableStudents.Count == 0)
            //{
            //    Student Student = new Student();
            //    Student.StudentList();
            //    ObservableStudents.AddStudent(Student);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
            //}
        }

        private void FilterStudents(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableStudents = new ObservableCollection<Student>(_allStudents);
                return;
            }
            else
            {
                ObservableStudents = new ObservableCollection<Student>(_allStudents.Where(t => t.FullName.ToLower().Contains(searchInput.ToLower())));
            }
        }

        private void RemoveStudentFromPresentationAndManager(Student Student)
        {
            ObservableStudents.Remove(Student);
            _manager.RemoveStudent(Student);
        }

        private void OnDelete(Student Student)
        {
            DeleteRequested(Student);
        }

        //TODO: RemoveStudent below method - its a temp method for the AddStudent Student > Charting workflow
        //private void OnAddStudent(Student Student)
        //{
        //    Student.StudentNumber += 1;
        //    PropertyChanged(this, new PropertyChangedEventArgs("Students"));
        //    AddStudentRequested(Student);
        //}

        private void OnAddStudent()
        {
            //place holder for the actual on add Student command for the actual on add Student button
            //the one above is linked to the chart button i believe...
            AddRequested(new Student());
        }

        private void OnEditStudent(Student Student)
        {
            EditRequested(Student);
        }


        //FIXME: THIS IS NEVER FALSE
        private bool CanDelete(Student Student)
        {
            //TODO: Selected Student doesn't seem to work here, and this isn't really needed...
            //return SelectedStudent != null;
            return true;
        }

        private void OnClearSearch()
        {
            SearchInput = null;
        }

        #endregion
    }
}
