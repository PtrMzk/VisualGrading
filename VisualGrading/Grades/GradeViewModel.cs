using VisualGrading.Presentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Helpers;

namespace VisualGrading.Grades
{
    public class GradeViewModel : BaseViewModel
    {
        #region Constructor

        public GradeViewModel(IGradeRepository repository)
        {
            _repository = repository;
            DeleteCommand = new RelayCommand<Grade>(OnDelete, CanDelete);
            AddCommand = new RelayCommand(OnAddGrade);
            EditCommand = new RelayCommand<Grade>(OnEditGrade);
            ClearSearchCommand = new RelayCommand(OnClearSearch);
            //DeleteRequested += RemoveGradeFromPresentationAndRepository;
        }

        #endregion

        #region Properties

        private IGradeRepository _repository;

        private ObservableCollection<Grade> _observableGrades;

        public ObservableCollection<Grade> ObservableGrades
        {
            get
            {
                return _observableGrades;
            }
            set
            {
                SetProperty(ref _observableGrades, value);
                PropertyChanged(this, new PropertyChangedEventArgs("ObservableGrades"));
            }
        }

        private List<Grade> _allGrades;

        private Grade _selectedGrade { get; set; }

        public Grade SelectedGrade
        {
            get
            {
                return _selectedGrade;
            }
            set
            {
                if (_selectedGrade != value)
                {
                    _selectedGrade = value;
                    DeleteCommand.RaiseCanExecuteChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("ObservableGrades"));
                }
            }
        }

        public RelayCommand<Grade> DeleteCommand { get; private set; }

        public RelayCommand AddCommand { get; private set; }

        public RelayCommand AddSeriesCommand { get; private set; }

        public RelayCommand<Grade> EditCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event Action<Grade> AddRequested = delegate { };
        public event Action<Grade> EditRequested = delegate { };
        public event Action<Grade> DeleteRequested = delegate { };

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
                FilterGrades(_searchInput);

            }
        }


        #endregion

        #region Methods

        public async void LoadGrades()
        {
            if (DesignerProperties.GetIsInDesignMode(
               new System.Windows.DependencyObject())) return;
            //var ObservableGrades = new List<GradeDTO>();
            _allGrades =
                await
                    _repository.GetGradesAsync();
            ObservableGrades = new ObservableCollection<Grade>(_allGrades);
            //if (ObservableGrades == null || ObservableGrades.Count == 0)
            //{
            //    GradeDTO GradeDTO = new GradeDTO();
            //    GradeDTO.GradeList();
            //    ObservableGrades.AddGrade(GradeDTO);

            PropertyChanged(this, new PropertyChangedEventArgs("ObservableGrades"));
            //}
        }

        private void FilterGrades(string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                ObservableGrades = new ObservableCollection<Grade>(_allGrades);
                return;
            }
            else
            {
                ObservableGrades = new ObservableCollection<Grade>(_allGrades.Where(g => g.Test.Subject.ToLower().Contains(searchInput.ToLower())));
            }
        }

        //TODO: should not be a way to remove grades from front end
        //private void RemoveGradeFromPresentationAndRepository(GradeDTO GradeDTO)
        //{
        //    //TODO: Make these use events to talk to the repository instead
        //    ObservableGrades.Remove(GradeDTO);
        //    _repository.RemoveGrade(GradeDTO);
        //}

        private void OnDelete(Grade Grade)
        {
            DeleteRequested(Grade);
        }

        //TODO: RemoveGrade below method - its a temp method for the AddGrade GradeDTO > Charting workflow
        //private void OnAddGrade(GradeDTO GradeDTO)
        //{
        //    GradeDTO.GradeNumber += 1;
        //    PropertyChanged(this, new PropertyChangedEventArgs("Grades"));
        //    AddGradeRequested(GradeDTO);
        //}

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


        //FIXME: THIS IS NEVER FALSE
        private bool CanDelete(Grade Grade)
        {
            //TODO: Selected GradeDTO doesn't seem to work here, and this isn't really needed...
            //return SelectedGrade != null;
            return true;
        }

        private void OnClearSearch()
        {
            SearchInput = null;
        }

        #endregion

    }
}
