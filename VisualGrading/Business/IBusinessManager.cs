using System.Collections.Generic;
using System.Threading.Tasks;
using VisualGrading.Grades;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Business
{
    public interface IBusinessManager
    {
        void InsertGrade(Grade grade);
        Task InsertGradeAsync(Grade grade);
        void UpdateGrade(Grade grade);
        Task UpdateGradeAsync(Grade grade);
        void UpdateStudent(Student student);
        Task UpdateStudentAsync(Student student);
        void InsertStudent(Student student);
        Task InsertStudentAsync(Student student);
        void UpdateTest(Test test);
        Task UpdateTestAsync(Test test);
        void InsertTest(Test test);
        Task InsertTestAsync(Test test);
        void InsertTestSeries(TestSeries tests);
        Task InsertTestSeriesAsync(TestSeries tests);
        void DeleteStudent(Student student);
        Task DeleteStudentAsync(Student student);
        void DeleteTest(Test test);
        Task DeleteTestAsync(Test test);
        List<Grade> GetGrades();
        Task<List<Grade>> GetGradesAsync();
        Task<List<Student>> GetStudentsAsync();
        List<Student> GetStudents();
        Task<List<Test>> GetTestsAsync();
        List<Test> GetTests();
        List<Grade> GetFilteredGrades(List<Test> testsToFilterOn);
        List<Grade> GetFilteredGrades(List<Student> studentsToFilterOn = null, List<Test> testsToFilterOn = null, string subject = null, string subCategory = null);
    }
}