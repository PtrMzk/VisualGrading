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
        public long ID
        {
            get { return _id; } 
            set { SetProperty(ref _id, value); }
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

        [Required]
        public int MaximumPoints
        {
            get { return _maximumPoints; }
            set { SetProperty(ref _maximumPoints, value); }
        }
        #endregion

        #region Private Properties

        private long _id;
        private string _subject;
        private string _subCategory;
        private string _name;
        private DateTime _date;
        private int _seriesNumber;
        private int _maximumPoints;

        #endregion
    }
}
