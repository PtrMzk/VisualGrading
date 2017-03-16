#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// IGradeManager.cs
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using VisualGrading.Students;
using VisualGrading.Tests;

#endregion

namespace VisualGrading.Grades
{
    public interface IGradeRepository
    {
        #region Properties

        List<Grade> GradeList { get; set; }

        #endregion

        #region Public Methods

        void AddGradeAsync(Grade Grade);
        void AddGradesByStudentAsync(Student student);
        void AddGradesByTestAsync(Test test);
        //List<GradeDTO> GenerateGrades(List<StudentDTO> students, List<TestDTO> tests);
        Task<List<Grade>> GetGradesAsync();
        void RemoveGradesByStudentAsync(Student studentToRemove);
        void RemoveGradesByTestAsync(Test testToRemove);
        void UpdateGradeAsync(Grade updatedGrade);

        #endregion

        event PropertyChangedEventHandler PropertyChanged;
    }
}