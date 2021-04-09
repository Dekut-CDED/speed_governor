using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Interfaces
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _dbcontext;

        internal DbSet<T> dbset;

        public Repository(DataContext DbContext)
        {
            _dbcontext = DbContext;
            this.dbset = _dbcontext.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<System.Func<T, bool>> filter = null, System.Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderby = null, string incluedproperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (incluedproperties != null)
            {
                foreach (var includeproperty in incluedproperties.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeproperty);
                }
            }

            if (orderby != null)
            {
                return orderby(query).ToList();
            }

            return query.ToList();

        }
        public T GetFirstOrDefault(Expression<System.Func<T, bool>> filter = null, string incluedproperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                dbset.Where(filter);
            }

            if (incluedproperties != null)
            {
                foreach (var includeproperty in incluedproperties.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeproperty);
                }
            }
            return query.FirstOrDefault();
        }

        public T Get(string id)
        {
            return dbset.Find(id);
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void Remove(string id)
        {
            T entityToremove = Get(id);
            Remove(entityToremove);
        }
    }
}