using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.ViewModelHelpers;

namespace VisualGrading.Students
{
    public class StudentViewModel : BaseViewModel
    {
        #region Constructor

        public StudentViewModel()
        {
            //_repository = repository;

            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            DeleteCommand = new RelayCommand<Student>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddStudent);
            EditCommand = new RelayCommand<Student>(OnEditStudent);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            ChartCommand = new RelayCommand<Student>(OnChartRequested);

            //AutoSaveCommand = new RelayCommand(OnRowEdit);
            DeleteRequested += DeleteStudent;
        }

        #endregion

        #region Properties

        //private IStudentRepository _repository;


        private readonly IBusinessManager _businessManager;

        private ObservableCollectionExtended<Student> _observableStudents;

        public ObservableCollectionExtended<Student> ObservableStudents
        {
            get { return _observableStudents; }
            set
            {
                if ((_observableStudents != null) && (value != _observableStudents))
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
            get { return _selectedStudent; }
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

        public RelayCommand<Student> DeleteCommand { get; }

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Student> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public RelayCommand AutoSaveCommand { get; private set; }

        public RelayCommand<Student> ChartCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event Action<Student> AddRequested = delegate { };
        public event Action<Student> EditRequested = delegate { };
        public event Action<Student> DeleteRequested = delegate { };
        public event Action<Student> ChartRequested = delegate { };


        private string _searchInput;

        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterStudents(_searchInput);
            }
        }

        public void OnRowEdit(object sender, PropertyChangedEventArgs e)
        {
            _businessManager.UpdateStudentAsync((Student) sender);
        }

        #endregion

        #region Methods

        public async void LoadStudents()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            //todo: this wont update if studnet is added 
            //if (_allStudents == null)
            {
                 _allStudents = await _businessManager.GetStudentsAsync();

                ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);

                PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
            }
        }

        private void ObservableStudents_CollectionChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _businessManager.UpdateStudentAsync((Student) sender);
        }

        private void FilterStudents(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
                ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);
            else
                ObservableStudents =
                    new ObservableCollectionExtended<Student>(
                        _allStudents.Where(t => t.FullName.ToLower().Contains(searchInput.ToLower())));
        }

        private void DeleteStudent(Student student)
        {
            ObservableStudents.Remove(student);
            _allStudents.Remove(student);
            _businessManager.DeleteStudentAsync(student);
        }

        private void OnDelete(Student student)
        {
            DeleteRequested(student);
        }
        

        private void OnAddStudent()
        {
            //place holder for the actual on add StudentDTO command for the actual on add StudentDTO button
            //the one above is linked to the chart button i believe...
            AddRequested(new Student());
        }

        private void OnEditStudent(Student student)
        {
            EditRequested(student);
        }

        private void OnChartRequested(Student student)
        {
            ChartRequested(student);

        }


        //TODO: THIS IS NEVER FALSE
        private bool CanDelete(Student student)
        {
            //TODO: Selected StudentDTO doesn't seem to work here, and this isn't really needed...
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