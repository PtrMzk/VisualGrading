using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Grades
{
    public class GradeViewModel : BaseViewModel
    {
        #region Constructor

        public GradeViewModel()
        {
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            AddCommand = new RelayCommand(OnAddGrade);
            EditCommand = new RelayCommand<Grade>(OnEditGrade);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            //DeleteRequested += RemoveGradeFromPresentationAndRepository;
        }

        #endregion

        #region Properties

        private readonly IBusinessManager _businessManager;

        private ObservableCollectionExtended<Grade> _observableGrades;

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

        private List<Grade> _allGrades;

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

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Grade> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event Action<Grade> AddRequested = delegate { };
        public event Action<Grade> EditRequested = delegate { };

        private string _searchInput;

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

        #region Methods

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

        private async void ObservableGrades_CollectionChanged(object sender,
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            await _businessManager.UpdateGradeAsync((Grade) sender);
        }

        private void FilterGrades(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
                ObservableGrades = new ObservableCollectionExtended<Grade>(_allGrades);
            else
                ObservableGrades =
                    new ObservableCollectionExtended<Grade>(
                        _allGrades.Where(g =>
                            g.Test.Name.ToLower().Contains(searchInput.ToLower()) ||
                            g.Test.Subject.ToLower().Contains(searchInput.ToLower()) ||
                            g.Test.SubCategory.ToLower().Contains(searchInput.ToLower()) ||
                            g.Student.FullName.ToLower().Contains(searchInput.ToLower())
                        )
                    );
        }

        private void OnAddGrade()
        {
            //place holder for the actual on add GradeDTO command for the actual on add GradeDTO button
            //the one above is linked to the chart button i believe...
            AddRequested(new Grade());
        }

        private void OnEditGrade(Grade Grade)
        {
            EditRequested(Grade);
        }

        private void OnClearSearch()
        {
            SearchInput = null;
        }

        #endregion
    }
}