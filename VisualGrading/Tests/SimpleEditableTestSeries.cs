#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// SimpleEditableTestSeries.cs
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

using System.ComponentModel.DataAnnotations;
using VisualGrading.Presentation;

#endregion

namespace VisualGrading.Tests
{
    public class SimpleEditableTestSeries : ValidatableBaseViewModel
    {
        #region Fields

        private long _id;
        private int _length;
        private int _maximumPoints;
        private string _name;
        private string _subCategory;
        private string _subject;

        #endregion

        #region Properties

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
    }
}