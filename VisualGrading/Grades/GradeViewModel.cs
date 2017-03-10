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

                ObservableGrades =
                    new ObservableCollectionExtended<Grade>(
                        Enumerable.Where(_allGrades, g => matchingIDs.Contains(g.ID)));
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
            SearchInput = null;
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

        #endregion

        public event Action<Grade> AddRequested = delegate { };
        public event Action<Grade> EditRequested = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}