#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// IBusinessManager.cs
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
using System.Threading.Tasks;
using VisualGrading.Grades;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

#endregion

namespace VisualGrading.Business
{
    public interface IBusinessManager
    {
        #region Public Methods

        void DeleteStudent(Student student);
        Task DeleteStudentAsync(Student student);
        void DeleteTest(Test test);
        Task DeleteTestAsync(Test test);
        List<Grade> GetFilteredGrades(List<Test> testsToFilterOn);
        List<Grade> GetFilteredGrades(string subject, string subCategory);

        List<Grade> GetFilteredGrades(List<Student> studentsToFilterOn = null, List<Test> testsToFilterOn = null,
            string subject = null, string subCategory = null);

        List<Grade> GetGrades();
        Task<List<Grade>> GetGradesAsync();
        SettingsProfile GetSettingsProfileWithoutPassword();
        Task<SettingsProfile> GetSettingsProfileWithoutPasswordAsync();
        List<Student> GetStudents();
        Task<List<Student>> GetStudentsAsync();
        List<Test> GetTests();
        Task<List<Test>> GetTestsAsync();
        void InsertGrade(Grade grade);
        Task InsertGradeAsync(Grade grade);
        void InsertSettingsProfile(SettingsProfile settingsProfile);
        Task InsertSettingsProfileAsync(SettingsProfile settingsProfile);
        void InsertStudent(Student student);
        Task InsertStudentAsync(Student student);
        void InsertTest(Test test);
        Task InsertTestAsync(Test test);
        void InsertTestSeries(TestSeries tests);
        Task InsertTestSeriesAsync(TestSeries tests);
        Task SendEmail(Student student);
        Task SendTestEmail(SettingsProfile settingsProfile);

        void UpdateGrade(Grade grade);
        Task UpdateGradeAsync(Grade grade);
        void UpdateSettingsProfile(SettingsProfile settingsProfile);
        Task UpdateSettingsProfileAsync(SettingsProfile settingsProfile);
        void UpdateStudent(Student student);
        Task UpdateStudentAsync(Student student);
        void UpdateTest(Test test);
        Task UpdateTestAsync(Test test);

        #endregion
    }
}