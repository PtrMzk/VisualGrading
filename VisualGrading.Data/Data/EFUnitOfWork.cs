//https://codefizzle.wordpress.com/2012/07/26/correct-use-of-repository-and-unit-of-work-patterns-in-asp-net-mvc/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Model.Repositories;

namespace VisualGrading.Model.Data
{
   public class EFUnitOfWork : DbContext, IUnitOfWork
    {
        private readonly EFRepository<Test> _testRepository;
        private readonly EFRepository<Student> _studentRepository;
        private readonly EFRepository<Grade> _gradeRepository;

        public DbSet<Test> Tests { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public EFUnitOfWork()
            : base("name=VisualGradingDBContext")
        {
            _testRepository = new EFRepository<Test>(Tests);
            _studentRepository = new EFRepository<Student>(Students);
            _gradeRepository = new EFRepository<Grade>(Grades);
        }

        #region IUnitOfWork Implementation

        public IRepository<Test> TestRepository
        {
            get { return _testRepository; }
        }

        public IRepository<Student> StudentRepository
        {
            get { return _studentRepository; }
        }

        public IRepository<Grade> GradeRepository
        {
            get { return _gradeRepository; }
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        #endregion
    }
}
