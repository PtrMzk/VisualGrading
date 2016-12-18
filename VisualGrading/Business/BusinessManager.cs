using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Helpers;
using VisualGrading.Model.Data;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Business
{
    public class BusinessManager : IBusinessManager
    {
        public BusinessManager()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
        }

        IDataManager _dataManager;

        #region Test Methods

        public async Task AddTestAsync(Test test)
        {
            _dataManager.SaveTest(test);
            
            //todo: see if you can do this from the repository
            var students = _dataManager.GetStudents();

            foreach (var student in students)
            {
                var grade = new Grade(student.ID, test.ID);
                _dataManager.SaveGrade(grade);

            }

            await _dataManager.CommitChangesAsync();
        }
        #endregion

        #region Student Methods
        public async Task AddStudentAsync(Student student)
        {
            _dataManager.SaveStudent(student);

            //todo: see if you can do this from the repository
            var tests = _dataManager.GetTests();

            foreach (var test in tests)
            {
                var grade = new Grade(student.ID, test.ID);
                _dataManager.SaveGrade(grade);

            }

            await _dataManager.CommitChangesAsync();
        }
        #endregion

    }
}
