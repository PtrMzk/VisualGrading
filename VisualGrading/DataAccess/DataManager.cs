using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Model.Data;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.DataAccess
{
    public class DataManager : IDataManager
    {
        #region Fields

        private readonly AutoMapperProfile _autoMapperProfile = new AutoMapperProfile();

        private readonly IRepository<GradeDTO> _gradeRepository;

        private readonly IRepository<StudentDTO> _studentRepository;

        // private readonly IRepository<IEntity> _studentRepositoryGen;

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

            //_studentRepositoryGen = _unitOfWork.StudentRepositoryGen;
        }

        #endregion

        #region Properties

        public static DataManager Instance { get; } = new DataManager();

        private SettingRepository _settingRepository => SettingRepository.Instance;

        #endregion

        #region Methods

        public void CommitChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task CommitChangesAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            var studentDTOs = await _studentRepository.GetAllAsync();

            var students = ConvertStudentDTOsToStudents(studentDTOs);

            return await CalculateOverallGradesAsync(students);
        }

        public List<Student> GetStudents()
        {
            var studentDTOs = _studentRepository.GetAll();

            var students = ConvertStudentDTOsToStudents(studentDTOs);

            return CalculateOverallGrades(students);
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

        public void DeleteStudent(Student student)
        {
            var studentDTO = new StudentDTO();

            Mapper.Map(student, studentDTO);

            var existingEntity = _studentRepository.Single(x => x.ID == studentDTO.ID);

            if (existingEntity != null)
                _studentRepository.Delete(existingEntity);
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

        private List<Student> CalculateOverallGrades(List<Student> students)
        {
            var grades = GetGrades();

            return CalculateOverallGrades(students, grades);
        }

        private async Task<List<Student>> CalculateOverallGradesAsync(List<Student> students)
        {
            var grades = await GetGradesAsync();

            return CalculateOverallGrades(students, grades);
        }

        private List<Student> CalculateOverallGrades(List<Student> students, List<Grade> grades)
        {
            var studentPoints = new Dictionary<long, StudentPointsHelper>();

            foreach (var grade in grades)
                if (studentPoints.ContainsKey(grade.StudentID))
                {
                    studentPoints[grade.StudentID].PointsAchieved += grade.Points;
                    studentPoints[grade.StudentID].MaxPoints += grade.Test.MaximumPoints;
                }
                else
                {
                    studentPoints.Add(grade.StudentID,
                        new StudentPointsHelper {PointsAchieved = grade.Points, MaxPoints = grade.Test.MaximumPoints});
                }

            foreach (var student in students)
                if (studentPoints.ContainsKey(student.ID))
                    student.OverallGrade = studentPoints[student.ID].Average;

            return students;
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            var testDTOs = await _testRepository.GetAllAsync();

            return ConvertTestDTOsToTests(testDTOs);
        }

        public List<Test> GetTests()
        {
            var testDTOs = _testRepository.GetAll();

            return ConvertTestDTOsToTests(testDTOs);
        }

        public void DeleteTest(Test test)
        {
            var testDTO = new TestDTO();

            Mapper.Map(test, testDTO);

            var existingEntity = _testRepository.Single(x => x.ID == testDTO.ID);
            if (existingEntity != null)
                _testRepository.Delete(existingEntity);
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

        public async Task<List<Grade>> GetGradesAsync()
        {
            var gradeDTOs = await _gradeRepository.GetAllAsync();

            return ConvertGradeDTOsToGrades(gradeDTOs);
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

        public List<Grade> GetGrades()
        {
            var gradeDTOs = _gradeRepository.GetAll();

            return ConvertGradeDTOsToGrades(gradeDTOs);
        }

        public void DeleteGrade(Grade grade)
        {
            var gradeDTO = new GradeDTO();

            Mapper.Map(grade, gradeDTO);

            var existingEntity = _gradeRepository.Single(x => x.ID == gradeDTO.ID);
            if (existingEntity != null)
                _gradeRepository.Delete(existingEntity);
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