using VisualGrading.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Helpers;

namespace VisualGrading.Tests
{
    public class SimpleEditableTestSeries : ValidatableBaseViewModel
    {

        #region Public Properties
        [Required]
        public Guid SeriesID
        {
            get { return _seriesID; }
            set { SetProperty(ref _seriesID, value); }
        }

        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
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
        public int Length
        {
            get { return _length; }
            set { SetProperty(ref _length, value); }
        }

        #endregion

        #region Private Properties

        private Guid _seriesID;
        private string _name; 
        private string _subject;
        private string _subCategory;
        private int _length;


        #endregion
    }
}
