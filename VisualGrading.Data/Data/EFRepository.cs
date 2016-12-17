//https://codefizzle.wordpress.com/2012/07/26/correct-use-of-repository-and-unit-of-work-patterns-in-asp-net-mvc/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Model.Repositories;

namespace VisualGrading.Model.Data
{
    public class EFRepository<T> : IRepository<T>
                                      where T : class, IEntity
    {
        private readonly DbSet<T> _dbSet;

        public EFRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        #region IGenericRepository<T> implementation

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.Where(x => true);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T FirstOrDefault()
        {
            return _dbSet.FirstOrDefault();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Attach(T entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
