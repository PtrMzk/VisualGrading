#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// GradeViewModel.cs
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
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Presentation;
using VisualGrading.Search;

#endregion

namespace VisualGrading.Grades
{
    public class GradeViewModel : BaseViewModel
    {
        #region Fields

        private readonly IBusinessManager _businessManager;

        private readonly IFileDialog _fileDialog;

        private List<Grade> _allGrades;

        private ObservableCollectionExtended<Grade> _observableGrades;

        private string _searchInput;

        #endregion

        #region Constructors

        public GradeViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            _fileDialog = ContainerHelper.Container.Resolve<IFileDialog>();

            AddCommand = new RelayCommand(OnAddGrade);
            ExportCommand = new RelayCommand(OnExport);
            EditCommand = new RelayCommand<Grade>(OnEditGrade);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            GoToStudentCommand = new RelayCommand<long>(OnGoToStudent);
            GoToTestCommand = new RelayCommand<long>(OnGoToTest);
        }

        #endregion

        #region Properties

        public ObservableCollectionExtended<Grade> ObservableGrades
        {
            get { return _observableGrades; }
            set
            {
                if (_observableGrades != null && value != _observableGrades)
                    _observableGrades.CollectionPropertyChanged -= ObservableGrades_CollectionChanged;

                SetProperty(ref _observableGrades, value);
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableGrades"));
                _observableGrades.CollectionPropertyChanged += ObservableGrades_CollectionChanged;
            }
        }

        private Grade _selectedGrade { get; set; }

        public Grade SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                if (_selectedGrade != value)
                {
                    _selectedGrade = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ObservableGrades"));
                }
            }
        }

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand ExportCommand { get; private set; }

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Grade> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public RelayCommand<long> GoToStudentCommand { get; private set; }

        public RelayCommand<long> GoToTestCommand { get; private set; }

        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                SetProperty(ref _searchInput, value);
                FilterGrades(_searchInput);
            }
        }

        #endregion

        #region Public Methods

        public void ClearSearch()
        {
            SearchInput = null;
        }

        public async void LoadGrades()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new DependencyObject())) return;

            //todo: does grades always need to be refreshed? ...
            //todo: ...if a student or test is changed it doesnt get reflected with the below if statement
            //if (_allGrades == null)
            {
                _allGrades = await _businessManager.GetGradesAsync();

                ObservableGrades = new ObservableCollectionExtended<Grade>(_allGrades);

                PropertyChanged(this, new PropertyChangedEventArgs("ObservableGrades"));
            }

            //reapply search filter
            FilterGrades(SearchInput);
        }

        public void SearchGrades(string searchInput)
        {
            SearchInput = searchInput;
        }

        #endregion

        #region Private Methods

        private void FilterGrades(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableGrades = new ObservableCollectionExtended<Grade>(_allGrades);
            }
            else
            {
                var smartSearch = new SmartSearch<Grade>();
                var matchingIDs = smartSearch.Search(_allGrades, searchInput);

                if (matchingIDs.Count > 0)
                    ObservableGrades =
                        new ObservableCollectionExtended<Grade>(
                            _allGrades.Where(g => matchingIDs.Contains(g.ID)));
                else
                    ObservableGrades = new ObservableCollectionExtended<Grade>();
            }
        }

        private async void ObservableGrades_CollectionChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            await _businessManager.UpdateGradeAsync((Grade) sender);
        }

        private void OnAddGrade()
        {
            AddRequested(new Grade());
        }

        private void OnClearSearch()
        {
            ClearSearch();
        }

        private void OnEditGrade(Grade Grade)
        {
            EditRequested(Grade);
        }

        private void OnExport()
        {
            //    var type = typeof(Grade);
            //    var properties = type.GetProperties();

            _fileDialog.SaveFileDialog.Filter = "CSV|*.csv";
            _fileDialog.SaveFileDialog.Title = "Export Grades to CSV";

            if (_fileDialog.SaveFileDialog.ShowDialog() == true)
            {
                var csvExport = new CsvExport();

                foreach (var grade in _allGrades)
                {
                    csvExport.AddRow();
                    csvExport["StudentID"] = grade.StudentID;
                    csvExport["Student"] = grade.Student.FullName;
                    csvExport["TestID"] = grade.TestID;
                    csvExport["Test"] = grade.Test.Name;
                    csvExport["Date"] = grade.Test.Date;
                    csvExport["Maximum Points"] = grade.Test.MaximumPoints;
                    csvExport["Points Achieved"] = grade.Points;
                    csvExport["Percentage"] = grade.PercentAverage;
                }

                csvExport.ExportToFile(_fileDialog.SaveFileDialog.FileName);
            }
        }

        private void OnGoToStudent(long id)
        {
            GoToStudentRequested(id);
        }

        private void OnGoToTest(long id)
        {
            GoToTestRequested(id);
        }

        #endregion

        public event Action<Grade> AddRequested = delegate { };
        public event Action<Grade> EditRequested = delegate { };
        public event Action<long> GoToStudentRequested = delegate { };
        public event Action<long> GoToTestRequested = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}