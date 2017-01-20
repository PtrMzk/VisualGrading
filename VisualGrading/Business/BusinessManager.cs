using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Business
{
    public class BusinessManager : IBusinessManager
    {
        #region Fields

        private readonly IDataManager _dataManager;

        #endregion

        #region Constructors

        public BusinessManager()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
        }

        #endregion

        #region Methods

        public async Task InsertSettingsProfileAsync(SettingsProfile settingsProfile)
        {
            await UpdateSettingsProfileAsync(settingsProfile);

        }

        public void InsertSettingsProfile(SettingsProfile settingsProfile)
        {
            UpdateSettingsProfile(settingsProfile);

        }

        public void UpdateSettingsProfile(SettingsProfile settingsProfile)
        {
            _dataManager.SaveSettingsProfile(settingsProfile);
            _dataManager.CommitChanges();
        }

        public async Task UpdateSettingsProfileAsync(SettingsProfile settingsProfile)
        {
            _dataManager.SaveSettingsProfile(settingsProfile);
            _dataManager.CommitChanges();
        }

        public async Task<SettingsProfile> GetSettingsProfileAsync()
        {
            return await _dataManager.GetSettingsProfileAsync();
        }

        public SettingsProfile GetSettingsProfile()
        {
            return _dataManager.GetSettingsProfile();
        }



        public async Task UpdateTestAsync(Test test)
        {
            _dataManager.SaveTest(test);

            await _dataManager.CommitChangesAsync();
        }

        public void UpdateTest(Test test)
        {
            _dataManager.SaveTest(test);

            _dataManager.CommitChanges();
        }

        public void InsertTest(Test test)
        {
            InsertTestAndApplicableGrades(test);

            _dataManager.CommitChanges();
        }

        public async Task InsertTestAsync(Test test)
        {
            InsertTestAndApplicableGrades(test);

            await _dataManager.CommitChangesAsync();
        }

        public async Task InsertTestSeriesAsync(TestSeries tests)
        {
            for (var i = 0; i < tests.TestCount; i++)
            {
                var seriesNumber = i + 1;
                var test = GenerateTestFromTestSeries(tests, seriesNumber);
                await InsertTestAsync(test);
            }
        }

        public void InsertTestSeries(TestSeries tests)
        {
            for (var i = 0; i < tests.TestCount; i++)
            {
                var seriesNumber = i + 1;
                var test = GenerateTestFromTestSeries(tests, seriesNumber);
                InsertTest(test);
            }
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            return await _dataManager.GetTestsAsync();
        }

        public List<Test> GetTests()
        {
            return _dataManager.GetTests();
        }

        //the delete methods delete grades via Entity Framework
        public async Task DeleteTestAsync(Test test)
        {
            _dataManager.DeleteTest(test);

            await _dataManager.CommitChangesAsync();
        }

        public void DeleteTest(Test test)
        {
            _dataManager.DeleteTest(test);

            _dataManager.CommitChanges();
        }

        private Test GenerateTestFromTestSeries(TestSeries tests, int seriesNumber)
        {
            var test = new Test(string.Format("{0} {1}", tests.Name, seriesNumber), tests.Subject, tests.SubCategory,
                seriesNumber, tests.MaximumPoints);
            return test;
        }

        private void InsertTestAndApplicableGrades(Test test)
        {
            _dataManager.SaveTest(test);

            //todo: see if you can do this from the repository
            var students = _dataManager.GetStudents();

            foreach (var student in students)
            {
                var grade = new Grade(student.ID, test.ID);
                _dataManager.SaveGrade(grade);
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _dataManager.SaveStudent(student);

            await _dataManager.CommitChangesAsync();
        }

        public void UpdateStudent(Student student)
        {
            _dataManager.SaveStudent(student);

            _dataManager.CommitChanges();
        }

        public async Task InsertStudentAsync(Student student)
        {
            InsertStudentAndApplicableGrades(student);

            await _dataManager.CommitChangesAsync();
        }

        public void InsertStudent(Student student)
        {
            InsertStudentAndApplicableGrades(student);

            _dataManager.CommitChanges();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _dataManager.GetStudentsAsync();
        }

        public List<Student> GetStudents()
        {
            return _dataManager.GetStudents();
        }

        //the delete methods delete grades via Entity Framework
        public async Task DeleteStudentAsync(Student student)
        {
            _dataManager.DeleteStudent(student);

            await _dataManager.CommitChangesAsync();
        }

        public void DeleteStudent(Student student)
        {
            _dataManager.DeleteStudent(student);

            _dataManager.CommitChanges();
        }

        private void InsertStudentAndApplicableGrades(Student student)
        {
            _dataManager.SaveStudent(student);

            //todo: see if you can do this from the repository
            var tests = _dataManager.GetTests();

            foreach (var test in tests)
            {
                var grade = new Grade(student.ID, test.ID);
                _dataManager.SaveGrade(grade);
            }
        }

        //this can be the same as UpdateGrade since nothing needs to be generated
        public async Task InsertGradeAsync(Grade grade)
        {
            await UpdateGradeAsync(grade);
        }

        //this can be the same as UpdateGrade since nothing needs to be generated
        public void InsertGrade(Grade grade)
        {
            UpdateGrade(grade);
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            _dataManager.SaveGrade(grade);

            await _dataManager.CommitChangesAsync();
        }

        public void UpdateGrade(Grade grade)
        {
            _dataManager.SaveGrade(grade);

            _dataManager.CommitChanges();
        }

        public async Task<List<Grade>> GetGradesAsync()
        {
            return await _dataManager.GetGradesAsync();
        }

        public List<Grade> GetFilteredGrades(List<Test> testsToFilterOn)
        {
            return GetFilteredGrades(null, testsToFilterOn);
        }

        public List<Grade> GetFilteredGrades(string subject, string subCategory)
        {
            return GetFilteredGrades(null, null, subject, subCategory);
        }

        public List<Grade> GetFilteredGrades(List<Student> studentsToFilterOn = null, List<Test> testsToFilterOn = null,
            string subject = null, string subCategory = null)
        {
            var studentIDsToFilterOn = new List<long>();
            var testIDsToFilterOn = new List<long>();

            if (studentsToFilterOn != null && studentsToFilterOn.Count > 0)
                studentsToFilterOn.ForEach(s => studentIDsToFilterOn.Add(s.ID));

            if (testsToFilterOn != null && testsToFilterOn.Count > 0)
                testsToFilterOn.ForEach(t => testIDsToFilterOn.Add(t.ID));

            return _dataManager.GetFilteredGrades(studentIDsToFilterOn, testIDsToFilterOn, subject, subCategory);
        }

        public List<Grade> GetGrades()
        {
            return _dataManager.GetGrades();
        }

        #endregion
    }
}