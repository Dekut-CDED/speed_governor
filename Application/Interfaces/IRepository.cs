using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(string Id);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string incluedproperties = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string incluedproperties = null);
        void Add(T entity);
        void Remove(T entity);
        void Remove(string id);
    }
}