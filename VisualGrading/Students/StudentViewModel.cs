#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// StudentViewModel.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

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

#endregion

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
            GoToStudentGradesCommand = new RelayCommand<long>(OnGoToStudentGrades);

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

        public RelayCommand<long> GoToStudentGradesCommand { get; private set; }

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

        public void ClearSearch()
        {
            SearchInput = null;
        }

        public async void LoadStudents()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            _allStudents = await _businessManager.GetStudentsAsync();

            ObservableStudents = new ObservableCollectionExtended<Student>(_allStudents);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableStudents"));

            //reapply search filter
            FilterStudents(SearchInput);
        }

        public void OnRowEdit(object sender, PropertyChangedEventArgs e)
        {
            _businessManager.UpdateStudentAsync((Student) sender);
        }

        public void SearchStudents(string searchInput)
        {
            SearchInput = searchInput;
        }

        #endregion

        #region Private Methods

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
            ClearSearch();
        }

        private void OnDelete(Student student)
        {
            DeleteRequested(student);
        }

        private void OnEditStudent(Student student)
        {
            EditRequested(student);
        }

        private void OnGoToStudentGrades(long id)
        {
            GoToStudentGradesRequested(id);
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
        public event Action<long> GoToStudentGradesRequested = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}