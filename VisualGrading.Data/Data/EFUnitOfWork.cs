#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// EFUnitOfWork.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System.Data.Entity;
using System.Threading.Tasks;
using SQLite.CodeFirst;

#endregion

namespace VisualGrading.Model.Data
{
    public class EFUnitOfWork : DbContext, IUnitOfWork
    {
        #region Fields

        //private readonly IRepository<IEntity> _studentRepositoryGen;
        private readonly EFRepository<GradeDTO> _gradeRepository;
        private readonly EFRepository<SettingsProfileDTO> _settingsProfileRepository;
        private readonly EFRepository<StudentDTO> _studentRepository;
        private readonly EFRepository<TestDTO> _testRepository;

        #endregion

        #region Constructors

        public EFUnitOfWork()
            : base("name=VisualGradingDBContext")
        {
            _testRepository = new EFRepository<TestDTO>(Tests);
            _studentRepository = new EFRepository<StudentDTO>(Students);
            _gradeRepository = new EFRepository<GradeDTO>(Grades);
            _settingsProfileRepository = new EFRepository<SettingsProfileDTO>(SettingsProfile);

            //_studentRepositoryGen = new EFRepository<IEntity>();
        }

        #endregion

        #region Properties

        public DbSet<TestDTO> Tests { get; set; }
        public DbSet<StudentDTO> Students { get; set; }
        public DbSet<GradeDTO> Grades { get; set; }
        public DbSet<SettingsProfileDTO> SettingsProfile { get; set; }

        public IRepository<TestDTO> TestRepository
        {
            get { return _testRepository; }
        }

        public IRepository<SettingsProfileDTO> SettingsProfileRepository
        {
            get { return _settingsProfileRepository; }
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

        #endregion

        #region Public Methods

        public void Commit()
        {
            SaveChanges();
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync();
        }

        #endregion

        #region Private Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<EFUnitOfWork>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        #endregion
    }
}