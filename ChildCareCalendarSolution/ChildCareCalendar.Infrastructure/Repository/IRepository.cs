using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
