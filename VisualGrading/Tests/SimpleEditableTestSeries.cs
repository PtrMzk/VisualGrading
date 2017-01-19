using VisualGrading.ViewModelHelpers;
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
        public long ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
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

        [Required]
        public int MaximumPoints
        {
            get { return _maximumPoints; }
            set { SetProperty(ref _maximumPoints, value); }
        }

        #endregion

        #region Private Properties

        private long _id;
        private string _name; 
        private string _subject;
        private string _subCategory;
        private int _length;
        private int _maximumPoints;



        #endregion
    }
}
