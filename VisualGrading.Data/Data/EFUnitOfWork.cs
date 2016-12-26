//https://codefizzle.wordpress.com/2012/07/26/correct-use-of-repository-and-unit-of-work-patterns-in-asp-net-mvc/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;
using VisualGrading.Model.Data;

namespace VisualGrading.Model.Data
{
   public class EFUnitOfWork : DbContext, IUnitOfWork
    {
        private readonly EFRepository<TestDTO> _testRepository;
        private readonly EFRepository<StudentDTO> _studentRepository;
        //private readonly IRepository<IEntity> _studentRepositoryGen;
        private readonly EFRepository<GradeDTO> _gradeRepository;

        public DbSet<TestDTO> Tests { get; set; }
        public DbSet<StudentDTO> Students { get; set; }
        public DbSet<GradeDTO> Grades { get; set; }

        public EFUnitOfWork()
            : base("name=VisualGradingDBContext")
        {
            _testRepository = new EFRepository<TestDTO>(Tests);
            _studentRepository = new EFRepository<StudentDTO>(Students);
            _gradeRepository = new EFRepository<GradeDTO>(Grades);
            //_studentRepositoryGen = new EFRepository<IEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<EFUnitOfWork>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        #region IUnitOfWork Implementation

        public IRepository<TestDTO> TestRepository
        {
            get { return _testRepository; }
        }

        public IRepository<StudentDTO> StudentRepository
        {
            get { return _studentRepository; }
        }

        //public IRepository<IEntity> StudentRepositoryGen
        //{
        //    get { return _studentRepositoryGen; }
        //}

        public IRepository<GradeDTO> GradeRepository
        {
            get { return _gradeRepository; }
        }
        
        public void Commit()
        {
            this.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await this.SaveChangesAsync();
        }

        #endregion
    }
}
