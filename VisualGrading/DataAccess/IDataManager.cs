using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using VisualGrading.Grades;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.DataAccess
{
    public interface IDataManager
    {
        Task CommitChangesAsync();
        void CommitChanges();
        
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
        List<Grade> GetFilteredGrades(List<long> studentIDsToFilterOn = null, List<long> testIDsToFilterOn = null, string subject = null, string subCategory = null);

        void SaveSettingsProfile(SettingsProfile settingsProfile);

        SettingsProfile GetSettingsProfile();

        Task<SettingsProfile> GetSettingsProfileAsync();
    }
}