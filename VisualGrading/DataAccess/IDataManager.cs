using System.Collections.Generic;
using System.Threading.Tasks;
using VisualGrading.Grades;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.DataAccess
{
    public interface IDataManager
    {

        Task CommitChangesAsync();
        void CommitChanges();


        //T Load<T>();
        //Task<T> LoadAsync<T>();
        //void Save<T>(object objectToSave);
        //Task SaveAsync<T>(object objectToSave);

        Task<List<Student>> GetStudentsAsync();
        List<Student> GetStudents();
        void SaveStudent(Student student);
        void DeleteStudent(Student student);
        

        Task<List<Test>> GetTestsAsync();
        List<Test> GetTests();
        void SaveTest(Test test);
        void DeleteTest(Test test);
        

        Task<List<Grade>> GetGradesAsync();
        List<Grade> GetGrades();
        void SaveGrade(Grade grade);
        void DeleteGrade(Grade grade);
        List<Grade> GetFilteredGrades(List<long> studentIDsToFilterOn = null, List<long> testIDsToFilterOn = null);

        //Task UpdateAsync<T>(T businessObject);
    }
}