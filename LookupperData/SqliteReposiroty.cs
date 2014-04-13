using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookupperData
{
    public class SqliteReposiroty<T> : IDisposable, IRepository<T> where T : class
    {
        private readonly DbContext dbContext;
        protected DbSet<T> DbSet;

        public SqliteReposiroty(DbContext context)
        {
            dbContext = context;
            DbSet = context.Set<T>();
        }

        public void Insert(T entity)
        {
            DbSet.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            Save();
        }

        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.AsEnumerable();
        }

        public void Save()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                // log here
                throw;
            }
        }

        public void Dispose()
        {
            DbSet = null;
            dbContext.Dispose();
        }
    }
}
