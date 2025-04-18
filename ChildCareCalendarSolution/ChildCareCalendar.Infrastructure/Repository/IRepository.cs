﻿using ChildCareCalendar.Domain.Entities;
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
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetQueryable();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, object key);
        Task DeleteAsync(T entity);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Task<T> Add(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

    }
}
