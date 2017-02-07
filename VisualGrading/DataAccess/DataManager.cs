using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Model.Data;
using VisualGrading.Settings;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.DataAccess
{
    public class DataManager : IDataManager
    {
        #region Fields

        private readonly AutoMapperProfile _autoMapperProfile = new AutoMapperProfile();

        private readonly Encryption _encryption;

        private readonly IRepository<GradeDTO> _gradeRepository;

        private readonly IRepository<SettingsProfileDTO> _settingsProfileRepository;

        private readonly IRepository<StudentDTO> _studentRepository;

        private readonly IRepository<TestDTO> _testRepository;

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        private DataManager()
        {
            _unitOfWork = ContainerHelper.Container.Resolve<IUnitOfWork>();

            _studentRepository = _unitOfWork.StudentRepository;

            _testRepository = _unitOfWork.TestRepository;

            _gradeRepository = _unitOfWork.GradeRepository;
            _settingsProfileRepository = _unitOfWork.SettingsProfileRepository;

            _encryption = new Encryption();
        }

        #endregion

        #region Properties

        public static DataManager Instance { get; } = new DataManager();

        #endregion

        #region Public Methods

        //TODO: Refactor these methods to be generic
        public void CommitChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task CommitChangesAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public void DeleteGrade(Grade grade)
        {
            var gradeDTO = new GradeDTO();

            Mapper.Map(grade, gradeDTO);

            var existingEntity = _gradeRepository.Single(x => x.ID == gradeDTO.ID);
            if (existingEntity != null)
                _gradeRepository.Delete(existingEntity);
        }

        public void DeleteStudent(Student student)
        {
            var studentDTO = new StudentDTO();

            Mapper.Map(student, studentDTO);

            var existingEntity = _studentRepository.Single(x => x.ID == studentDTO.ID);

            if (existingEntity != null)
                _studentRepository.Delete(existingEntity);
        }

        public void DeleteTest(Test test)
        {
            var testDTO = new TestDTO();

            Mapper.Map(test, testDTO);

            var existingEntity = _testRepository.Single(x => x.ID == testDTO.ID);
            if (existingEntity != null)
                _testRepository.Delete(existingEntity);
        }

        public List<Grade> GetFilteredGrades(List<long> studentIDsFilter = null, List<long> testIDsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null)
        {
            List<GradeDTO> gradeDTOs;

            //TODO: Make sure null check is not needed for the two lists
            // it was breaking when implemeneted. 
            gradeDTOs =
                _gradeRepository.Find(
                    g =>
                        (studentIDsFilter.Contains(g.Student.ID) || studentIDsFilter.Count == 0)
                        && (testIDsFilter.Contains(g.Test.ID) || testIDsFilter.Count == 0)
                        && (g.Test.Subject == subjectFilter || subjectFilter == null)
                        && (g.Test.SubCategory == subCategoryFilter || subCategoryFilter == null)
                );

            return ConvertGradeDTOsToGrades(gradeDTOs);
        }

        public async Task<List<Grade>> GetFilteredGradesAsync(List<long> studentIDsFilter = null,
            List<long> testIDsFilter = null,
            string subjectFilter = null, string subCategoryFilter = null)
        {
            List<GradeDTO> gradeDTOs;

            if (studentIDsFilter == null)
                studentIDsFilter = new List<long>();

            if (testIDsFilter == null)
                testIDsFilter = new List<long>();

            //TODO: Make sure null check is not needed for the two lists
            // it was breaking when implemeneted. 
            gradeDTOs =
                await _gradeRepository.FindAsync(
                    g =>
                        (studentIDsFilter.Contains(g.Student.ID) || studentIDsFilter.Count == 0)
                        && (testIDsFilter.Contains(g.Test.ID) || testIDsFilter.Count == 0)
                        && (g.Test.Subject == subjectFilter || subjectFilter == null)
                        && (g.Test.SubCategory == subCategoryFilter || subCategoryFilter == null)
                );

            return ConvertGradeDTOsToGrades(gradeDTOs);
        }

        public List<Grade> GetGrades()
        {
            var gradeDTOs = _gradeRepository.GetAll();

            return ConvertGradeDTOsToGrades(gradeDTOs);
        }

        public async Task<List<Grade>> GetGradesAsync()
        {
            var gradeDTOs = await _gradeRepository.GetAllAsync();

            return ConvertGradeDTOsToGrades(gradeDTOs);
        }

        public SettingsProfile GetSettingsProfileWithoutPassword()
        {
            var settingsProfileDTO = _settingsProfileRepository.FirstOrDefault();
            return ConvertSettingsProfileDTOToSettingProfileWithoutPassword(settingsProfileDTO);
        }

        public async Task<SettingsProfile> GetSettingsProfileWithoutPasswordAsync()
        {
            var settingsProfileDTO = await _settingsProfileRepository.FirstOrDefaultAsync();
            return ConvertSettingsProfileDTOToSettingProfileWithoutPassword(settingsProfileDTO);
        }

        public SettingsProfile GetSettingsProfileWithPassword()
        {
            var settingsProfileDTO = _settingsProfileRepository.FirstOrDefault();
            return ConvertSettingsProfileDTOToSettingProfileWithPassword(settingsProfileDTO);
        }

        public async Task<SettingsProfile> GetSettingsProfileWithPasswordAsync()
        {
            var settingsProfileDTO = await _settingsProfileRepository.FirstOrDefaultAsync();
            return ConvertSettingsProfileDTOToSettingProfileWithPassword(settingsProfileDTO);
        }

        public List<Student> GetStudents()
        {
            var studentDTOs = _studentRepository.GetAll();

            var students = ConvertStudentDTOsToStudents(studentDTOs);

            return CalculateOverallGrades(students);
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            var studentDTOs = await _studentRepository.GetAllAsync();

            var students = ConvertStudentDTOsToStudents(studentDTOs);

            return await CalculateOverallGradesAsync(students);
        }

        public List<Test> GetTests()
        {
            var testDTOs = _testRepository.GetAll();

            return ConvertTestDTOsToTests(testDTOs);
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            var testDTOs = await _testRepository.GetAllAsync();

            return ConvertTestDTOsToTests(testDTOs);
        }

        public void SaveGrade(Grade grade)
        {
            var gradeDTO = new GradeDTO();
            GradeDTO existingEntity = null;

            Mapper.Map(grade, gradeDTO);

            if (gradeDTO.ID != 0)
                existingEntity = _gradeRepository.Single(x => x.ID == gradeDTO.ID);
            if (existingEntity != null)
                _unitOfWork.Entry(existingEntity).CurrentValues.SetValues(gradeDTO);
            else
                _gradeRepository.Add(gradeDTO);
        }

        public void SaveSettingsProfile(SettingsProfile settingsProfile)
        {
            var settingsProfileDTO = new SettingsProfileDTO();
            SettingsProfileDTO existingEntity = null;

            Mapper.Map(settingsProfile, settingsProfileDTO);

            settingsProfileDTO.EncryptedEmailPassword = EncryptEmailPassword(settingsProfile);

            if (settingsProfileDTO.ID != 0)
                existingEntity = _settingsProfileRepository.Single(x => x.ID == settingsProfileDTO.ID);
            if (existingEntity != null)
                _unitOfWork.Entry(existingEntity).CurrentValues.SetValues(settingsProfileDTO);
            else
                _settingsProfileRepository.Add(settingsProfileDTO);
        }

        public void SaveStudent(Student student)
        {
            var studentDTO = new StudentDTO();
            StudentDTO existingEntity = null;

            Mapper.Map(student, studentDTO);

            if (studentDTO.ID != 0)
                existingEntity = _studentRepository.Single(x => x.ID == studentDTO.ID);

            if (existingEntity != null)
                _unitOfWork.Entry(existingEntity).CurrentValues.SetValues(studentDTO);
            else
                _studentRepository.Add(studentDTO);
        }

        public void SaveTest(Test test)
        {
            var testDTO = new TestDTO();
            TestDTO existingEntity = null;

            Mapper.Map(test, testDTO);

            if (testDTO.ID != 0)
                existingEntity = _testRepository.Single(x => x.ID == testDTO.ID);
            if (existingEntity != null)
                _unitOfWork.Entry(existingEntity).CurrentValues.SetValues(testDTO);
            else
                _testRepository.Add(testDTO);
        }

        #endregion

        #region Private Methods

        private List<Student> CalculateOverallGrades(List<Student> students)
        {
            var grades = GetGrades();

            return CalculateOverallGrades(students, grades);
        }

        private List<Student> CalculateOverallGrades(List<Student> students, List<Grade> grades)
        {
            var studentPoints = new Dictionary<long, StudentPointsHelper>();

            foreach (var grade in grades)
                if (grade.Points != null)
                    if (studentPoints.ContainsKey(grade.StudentID))
                    {
                        studentPoints[grade.StudentID].PointsAchieved += grade.NonNullablePoints;
                        studentPoints[grade.StudentID].MaxPoints += grade.Test.MaximumPoints;
                    }
                    else
                    {
                        studentPoints.Add(grade.StudentID,
                            new StudentPointsHelper
                            {
                                PointsAchieved = grade.NonNullablePoints,
                                MaxPoints = grade.Test.MaximumPoints
                            });
                    }

            foreach (var student in students)
                if (studentPoints.ContainsKey(student.ID))
                    student.OverallGrade = studentPoints[student.ID].Average;

            return students;
        }

        private async Task<List<Student>> CalculateOverallGradesAsync(List<Student> students)
        {
            var grades = await GetGradesAsync();

            return CalculateOverallGrades(students, grades);
        }

        private List<Grade> ConvertGradeDTOsToGrades(List<GradeDTO> gradeDTOs)
        {
            var grades = new List<Grade>();

            foreach (var gradeDTO in gradeDTOs)
            {
                var grade = new Grade();
                Mapper.Map(gradeDTO, grade);
                grades.Add(grade);
            }

            return grades;
        }

        private SettingsProfile ConvertSettingsProfileDTOToSettingProfileWithoutPassword(
            SettingsProfileDTO settingProfileDTO)
        {
            var settingsProfile = new SettingsProfile();

            Mapper.Map(settingProfileDTO, settingsProfile);

            return settingsProfile;
        }

        private SettingsProfile ConvertSettingsProfileDTOToSettingProfileWithPassword(
            SettingsProfileDTO settingProfileDTO)
        {
            var settingsProfile = ConvertSettingsProfileDTOToSettingProfileWithoutPassword(settingProfileDTO);

            settingsProfile.EmailPassword = DecryptEmailPassword(settingProfileDTO);

            return settingsProfile;
        }

        private List<Student> ConvertStudentDTOsToStudents(List<StudentDTO> studentDTOs)
        {
            var students = new List<Student>();

            foreach (var studentDTO in studentDTOs)
            {
                var student = new Student();
                Mapper.Map(studentDTO, student);
                students.Add(student);
            }

            return students;
        }

        private List<Test> ConvertTestDTOsToTests(List<TestDTO> testDTOs)
        {
            var tests = new List<Test>();

            foreach (var testDTO in testDTOs)
            {
                var test = new Test();
                Mapper.Map(testDTO, test);
                tests.Add(test);
            }

            return tests;
        }

        private SecureString DecryptEmailPassword(SettingsProfileDTO settingsProfileDTO)
        {
            return _encryption.DecryptByteArray(settingsProfileDTO.EncryptedEmailPassword);
        }

        private byte[] EncryptEmailPassword(SettingsProfile settingsProfile)
        {
            return _encryption.EncryptSecureString(settingsProfile.EmailPassword);
        }

        #endregion

        //TODO: Is singleton needed? Might give constrained life to Unit of Work without it
    }

    internal class StudentPointsHelper
    {
        #region Properties

        public int PointsAchieved { get; set; }
        public int MaxPoints { get; set; }
        public decimal Average => PointsAchieved / (decimal) (MaxPoints == 0 ? 1 : MaxPoints);

        #endregion
    }
}