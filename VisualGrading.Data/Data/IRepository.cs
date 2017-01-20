﻿//Based on: https://codefizzle.wordpress.com/2012/07/26/correct-use-of-repository-and-unit-of-work-patterns-in-asp-net-mvc/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VisualGrading.Model.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> AsQueryable();

        List<T> GetAll();
        List<T> Find(Expression<Func<T, bool>> predicate);
        T Single(Expression<Func<T, bool>> predicate);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        T FirstOrDefault();
        Task<T> FirstOrDefaultAsync();


        Task<List<T>> GetAllAsync();

        void Add(T entity);
        void Delete(T entity);
        void Attach(T entity);
    }
}
