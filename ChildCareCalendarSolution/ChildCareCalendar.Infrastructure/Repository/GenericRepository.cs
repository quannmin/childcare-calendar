﻿using ChildCareCalendar.Domain.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ChildCareCalendarContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ChildCareCalendarContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>> predicate = null,
    Func<IQueryable<T>, IQueryable<T>> include = null
)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
