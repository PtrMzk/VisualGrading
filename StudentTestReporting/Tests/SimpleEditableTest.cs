using VisualGrading.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Helpers;

namespace VisualGrading.Tests
{
    public class SimpleEditableTest : ValidatableBaseViewModel
    {

        #region Public Properties
        [Required]
        public Guid TestID
        {
            get { return _testID; } 
            set { SetProperty(ref _testID, value); }
        }

        [Required]
        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

        [Required]
        public string SubCategory
        {
            get { return _subCategory; }
            set { SetProperty(ref _subCategory, value); }
        }

        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        [Required]
        public int SeriesNumber 
        {
            get { return _seriesNumber; }
            set { SetProperty(ref _seriesNumber, value); }
        }

        [Required]
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        #endregion

        #region Private Properties

        private Guid _testID;
        private string _subject;
        private string _subCategory;
        private string _name;
        private DateTime _date;
        private int _seriesNumber;

        #endregion
    }
}
