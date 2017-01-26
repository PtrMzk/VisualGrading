using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using VisualGrading.DataAccess;
using VisualGrading.Emails;
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
        private readonly IEmailManager _emailManager;

        #endregion

        #region Constructors

        public BusinessManager()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            _emailManager = ContainerHelper.Container.Resolve<IEmailManager>();
        }

        #endregion

        #region Public Methods

        public void DeleteStudent(Student student)
        {
            _dataManager.DeleteStudent(student);

            _dataManager.CommitChanges();
        }

        //the delete methods delete grades via Entity Framework
        public async Task DeleteStudentAsync(Student student)
        {
            _dataManager.DeleteStudent(student);

            await _dataManager.CommitChangesAsync();
        }

        public void DeleteTest(Test test)
        {
            _dataManager.DeleteTest(test);

            _dataManager.CommitChanges();
        }

        //the delete methods delete grades via Entity Framework
        public async Task DeleteTestAsync(Test test)
        {
            _dataManager.DeleteTest(test);

            await _dataManager.CommitChangesAsync();
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

        public async Task<List<Grade>> GetGradesAsync()
        {
            return await _dataManager.GetGradesAsync();
        }

        public SettingsProfile GetSettingsProfileWithoutPassword()
        {
            return _dataManager.GetSettingsProfileWithoutPassword();
        }

        public async Task<SettingsProfile> GetSettingsProfileWithoutPasswordAsync()
        {
            return await _dataManager.GetSettingsProfileWithoutPasswordAsync();
        }

        public List<Student> GetStudents()
        {
            return _dataManager.GetStudents();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _dataManager.GetStudentsAsync();
        }

        public List<Test> GetTests()
        {
            return _dataManager.GetTests();
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            return await _dataManager.GetTestsAsync();
        }

        //this can be the same as UpdateGrade since nothing needs to be generated
        public void InsertGrade(Grade grade)
        {
            UpdateGrade(grade);
        }

        //this can be the same as UpdateGrade since nothing needs to be generated
        public async Task InsertGradeAsync(Grade grade)
        {
            await UpdateGradeAsync(grade);
        }

        public void InsertSettingsProfile(SettingsProfile settingsProfile)
        {
            UpdateSettingsProfile(settingsProfile);
        }

        public async Task InsertSettingsProfileAsync(SettingsProfile settingsProfile)
        {
            await UpdateSettingsProfileAsync(settingsProfile);
        }

        public void InsertStudent(Student student)
        {
            InsertStudentAndApplicableGrades(student);

            _dataManager.CommitChanges();
        }

        public async Task InsertStudentAsync(Student student)
        {
            InsertStudentAndApplicableGrades(student);

            await _dataManager.CommitChangesAsync();
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

        public void InsertTestSeries(TestSeries tests)
        {
            for (var i = 0; i < tests.TestCount; i++)
            {
                var seriesNumber = i + 1;
                var test = GenerateTestFromTestSeries(tests, seriesNumber);
                InsertTest(test);
            }
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

        public async Task SendEmail(Student student)
        {
            var students = new List<long> {student.ID};

            Task<List<Grade>> gradesTask = _dataManager.GetFilteredGradesAsync(students);
            var grades = await gradesTask;
            await _emailManager.SendEmail(student, grades);
        }
        
        public async Task SendTestEmail(SettingsProfile settingsProfile)
        {
            await _emailManager.SendTestEmail(settingsProfile);
        }

        //todo: Email should be its own service

        public void UpdateGrade(Grade grade)
        {
            _dataManager.SaveGrade(grade);

            _dataManager.CommitChanges();
        }

        public async Task UpdateGradeAsync(Grade grade)
        {
            _dataManager.SaveGrade(grade);

            await _dataManager.CommitChangesAsync();
        }

        public void UpdateSettingsProfile(SettingsProfile settingsProfile)
        {
            _dataManager.SaveSettingsProfile(settingsProfile);
            _dataManager.CommitChanges();
        }

        public async Task UpdateSettingsProfileAsync(SettingsProfile settingsProfile)
        {
            _dataManager.SaveSettingsProfile(settingsProfile);
            await _dataManager.CommitChangesAsync();
        }

        public void UpdateStudent(Student student)
        {
            _dataManager.SaveStudent(student);

            _dataManager.CommitChanges();
        }

        public async Task UpdateStudentAsync(Student student)
        {
            _dataManager.SaveStudent(student);

            await _dataManager.CommitChangesAsync();
        }

        public void UpdateTest(Test test)
        {
            _dataManager.SaveTest(test);

            _dataManager.CommitChanges();
        }

        public async Task UpdateTestAsync(Test test)
        {
            _dataManager.SaveTest(test);

            await _dataManager.CommitChangesAsync();
        }

        #endregion

        #region Private Methods

        private Test GenerateTestFromTestSeries(TestSeries tests, int seriesNumber)
        {
            var test = new Test(string.Format("{0} {1}", tests.Name, seriesNumber), tests.Subject, tests.SubCategory,
                seriesNumber, tests.MaximumPoints);
            return test;
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

        #endregion
    }
}