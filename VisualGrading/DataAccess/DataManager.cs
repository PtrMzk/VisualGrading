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
        //TODO: Is singleton needed? Might give constrained life to Unit of Work without it

        #region Singleton Implementation

        private DataManager()
        {
            _unitOfWork = ContainerHelper.Container.Resolve<IUnitOfWork>();

            _studentRepository = _unitOfWork.StudentRepository;

            _testRepository = _unitOfWork.TestRepository;

            _gradeRepository = _unitOfWork.GradeRepository;

            //_studentRepositoryGen = _unitOfWork.StudentRepositoryGen;
        }

        public static DataManager Instance { get; } = new DataManager();

        #endregion

        #region Properties

        private readonly AutoMapperProfile _autoMapperProfile = new AutoMapperProfile();

        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<StudentDTO> _studentRepository;

        // private readonly IRepository<IEntity> _studentRepositoryGen;

        private readonly IRepository<TestDTO> _testRepository;

        private readonly IRepository<GradeDTO> _gradeRepository;

        private SettingRepository _settingRepository => SettingRepository.Instance;

        #endregion

        #region Methods

        #region Public DataManager Methods

        public void CommitChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task CommitChangesAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        #endregion

        #region Public Student Methods

        public async Task<List<Student>> GetStudentsAsync()
        {
            var studentDTOs = await _studentRepository.GetAllAsync();

            return ConvertStudentDTOsToStudents(studentDTOs);
        }

        public List<Student> GetStudents()
        {
            var studentDTOs = _studentRepository.GetAll();

            return ConvertStudentDTOsToStudents(studentDTOs);
        }

        public void SaveStudent(Student student)
        {
            var studentDTO = new StudentDTO();
            StudentDTO existingEntity = null;

            Mapper.Map(student, studentDTO);

            if (studentDTO.ID != null && studentDTO.ID != 0)
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

        #endregion

        #region Private Student Methods

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

        #endregion

        #region Public Test Methods 

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

            if (testDTO.ID != null && testDTO.ID != 0)
                existingEntity = _testRepository.Single(x => x.ID == testDTO.ID);
            if (existingEntity != null)
                _unitOfWork.Entry(existingEntity).CurrentValues.SetValues(testDTO);
            else
                _testRepository.Add(testDTO);
        }

        #endregion

        #region Private Test Methods

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

        #endregion

        #region Public Grade Methods 

        public async Task<List<Grade>> GetGradesAsync()
        {
            var gradeDTOs = await _gradeRepository.GetAllAsync();

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

            if (gradeDTO.ID != null && gradeDTO.ID != 0)
                existingEntity = _gradeRepository.Single(x => x.ID == gradeDTO.ID);
            if (existingEntity != null)
                _unitOfWork.Entry(existingEntity).CurrentValues.SetValues(gradeDTO);
            else
                _gradeRepository.Add(gradeDTO);
        }

        #endregion

        #region Private Grade Methods

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
        
        #endregion
    }
}