using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Presentation;
using VisualGrading.Search;

namespace VisualGrading.Students
{
    public class StudentViewModel : BaseViewModel
    {
        #region Fields

        //private IStudentRepository _repository;

        private readonly IBusinessManager _businessManager;

        private List<Student> _allStudents;

        private ObservableCollectionExtended<Student> _observableStudents;

        private string _searchInput;

        #endregion

        #region Constructors

        public StudentViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            DeleteCommand = new RelayCommand<Student>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddStudent);
            EditCommand = new RelayCommand<Student>(OnEditStudent);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            ChartCommand = new RelayCommand<Student>(OnChartRequested);
            SendEmailCommand = new RelayCommand<Student>(OnSendEmail);

            DeleteRequested += DeleteStudent;
        }

        #endregion

        #region Properties

        public ObservableCollectionExtended<Student> ObservableStudents
        {
            get { return _observableStudents; }
            set
            {
                if (_observableStudents != null && value != _observableStudents)
                    _observableStudents.CollectionPropertyChanged -= ObservableStudents_CollectionChanged;

                SetProperty(ref _observableStudents, value);
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
                _observableStudents.CollectionPropertyChanged += ObservableStudents_CollectionChanged;
            }
        }

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

        public RelayCommand<Student> SendEmailCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public RelayCommand AutoSaveCommand { get; private set; }

        public RelayCommand<Student> ChartCommand { get; private set; }

        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterStudents(_searchInput);
            }
        }

        #endregion

        #region Public Methods

        public async void LoadStudents()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _allStudents = await _businessManager.GetStudentsAsync();

            ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));
        }

        public void OnRowEdit(object sender, PropertyChangedEventArgs e)
        {
            _businessManager.UpdateStudentAsync((Student) sender);
        }

        #endregion

        #region Private Methods

        //TODO: THIS IS NEVER FALSE
        private bool CanDelete(Student student)
        {
            //TODO: Selected StudentDTO doesn't seem to work here, and this isn't really needed...
            //return SelectedStudent != null;
            return true;
        }

        private void DeleteStudent(Student student)
        {
            ObservableStudents.Remove(student);
            _allStudents.Remove(student);
            _businessManager.DeleteStudentAsync(student);
        }

        private void FilterStudents(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);
            }
            else
            {
                var smartSearch = new SmartSearch<Grade>();
                var matchingIDs = smartSearch.Search(_allStudents, searchInput);

                ObservableStudents =
                    new ObservableCollectionExtended<Student>(
                        _allStudents.Where(g => matchingIDs.Contains(g.ID)));
            }
        }

        private void ObservableStudents_CollectionChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _businessManager.UpdateStudentAsync((Student) sender);
        }

        private void OnAddStudent()
        {
            AddRequested(new Student());
        }

        private void OnChartRequested(Student student)
        {
            ChartRequested(student);
        }

        private void OnClearSearch()
        {
            SearchInput = null;
        }

        private void OnDelete(Student student)
        {
            DeleteRequested(student);
        }

        private void OnEditStudent(Student student)
        {
            EditRequested(student);
        }

        private void OnSendEmail(Student student)
        {
            _businessManager.SendEmail(student);
        }

        #endregion

        public event Action<Student> AddRequested = delegate { };
        public event Action<Student> ChartRequested = delegate { };
        public event Action<Student> DeleteRequested = delegate { };
        public event Action<Student> EditRequested = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}