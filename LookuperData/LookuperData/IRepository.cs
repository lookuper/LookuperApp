using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LookuperData
{
    public interface IRepository<T> where T : class
    {
        void Insert(T entity);
        void Delete(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();

        void Save();
    }
}
