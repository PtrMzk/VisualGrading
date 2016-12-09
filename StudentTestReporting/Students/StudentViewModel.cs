using VisualGrading.Presentation;
using VisualGrading.Students;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VisualGrading.Helpers;

namespace VisualGrading.Students
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
            //AutoSaveCommand = new RelayCommand(OnRowEdit);
            DeleteRequested += RemoveStudentFromPresentationAndManager;
        }
        #endregion

        #region Properties
        private IStudentManager _manager;

        private ObservableCollectionExtended<Student> _observableStudents;

        public ObservableCollectionExtended<Student> ObservableStudents
        {
            get
            {
                return _observableStudents;
            }
            set
            {
                if (_observableStudents!= null && value != _observableStudents)
                    _observableStudents.CollectionPropertyChanged -= ObservableStudents_CollectionChanged;

                SetProperty(ref _observableStudents, value);
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
                _observableStudents.CollectionPropertyChanged += ObservableStudents_CollectionChanged;
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
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedStudents"));
                }
            }
        }

        public RelayCommand<Student> DeleteCommand { get; private set; }

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Student> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public RelayCommand AutoSaveCommand { get; private set; }

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

        public void OnRowEdit(object sender, PropertyChangedEventArgs e)
        {
            _manager.UpdateStudentAsync((Student)sender);
        }


        #endregion

        #region Methods

        public async void LoadStudents()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;

            _allStudents =
                await
                    _manager.GetStudentsAsync();
            ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
        }

        private void ObservableStudents_CollectionChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _manager.UpdateStudentAsync((Student)sender); ;
        }

        private void FilterStudents(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);
                return;
            }
            else
            {
                ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents.Where(t => t.FullName.ToLower().Contains(searchInput.ToLower())));
            }
        }

        private void RemoveStudentFromPresentationAndManager(Student Student)
        {
            ObservableStudents.Remove(Student);
            _manager.RemoveStudent(Student);
        }

        private void OnDelete(Student student)
        {
            DeleteRequested(student);
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
            AddRequested(new Student() { StudentID = Guid.NewGuid() });
        }

        private void OnEditStudent(Student student)
        {
            EditRequested(student);
        }


        //FIXME: THIS IS NEVER FALSE
        private bool CanDelete(Student student)
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
