using System;
using System.ComponentModel.DataAnnotations;
using StudentTestReporting.Annotations;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Tests
{
    public class SimpleEditableTest : ValidatableBaseViewModel
    {
        #region Fields

        private DateTime _date;

        private long _id;
        private int _maximumPoints;
        private string _name;
        private int _seriesNumber;
        private string _subCategory;
        private string _subject;

        #endregion

        #region Constructors

        public SimpleEditableTest()
        {
        }

        #endregion

        #region Properties

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
    }
}