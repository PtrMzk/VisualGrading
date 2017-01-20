//https://codefizzle.wordpress.com/2012/07/26/correct-use-of-repository-and-unit-of-work-patterns-in-asp-net-mvc/

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Model.Data;

namespace VisualGrading.Model.Data
{
    public interface IUnitOfWork : IDisposable, IObjectContextAdapter
    {
  //      IRepository<IEntity> TestRepository { get; }
        //IRepository<IEntity> StudentRepositoryGen { get; }
//        IRepository<IEntity> GradeRepository { get; }

        IRepository<TestDTO> TestRepository { get; }
        IRepository<StudentDTO> StudentRepository { get; }
        IRepository<GradeDTO> GradeRepository { get; }
        IRepository<SettingsProfileDTO> SettingsProfileRepository { get; }


        #region Borrowed from DbContext 
        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        #endregion

        void Commit();
        Task CommitAsync();
    }
}
