#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// EFRepository.cs
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

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace VisualGrading.Model.Data
{
    public class EFRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        #region Fields

        private readonly DbSet<T> _dbSet;

        #endregion

        #region Constructors

        public EFRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        #endregion

        #region Public Methods

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault()
        {
            return _dbSet.FirstOrDefault();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            return await _dbSet.FirstOrDefaultAsync();
        }

        public List<T> GetAll()
        {
            return _dbSet.Where(x => true).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.Where(x => true).ToListAsync();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.First(predicate);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        #endregion
    }
}