#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// GradeDTO.cs
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
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace VisualGrading.Model.Data
{
    [Table("Grade")]
    public class GradeDTO : IEntity
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public int? Points { get; set; }

        public virtual TestDTO Test { get; set; }

        [InverseProperty("ID")]
        [ForeignKey("Test")]
        public long TestID { get; set; }

        public virtual StudentDTO Student { get; set; }

        [InverseProperty("ID")]
        [ForeignKey("Student")]
        public long StudentID { get; set; }

        #endregion
    }
}