using System.Collections.Generic;
using System.Threading.Tasks;
using VisualGrading.Grades;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

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