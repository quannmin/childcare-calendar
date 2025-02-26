﻿using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(AppUser user);
        Task UpdateUserAsync(AppUser user);
        Task<int> DeleteUserAsync(int id);
        Task<IEnumerable<AppUser>> FindUsersAsync(Expression<Func<AppUser, bool>> predicate,
                                                                        params Expression<Func<AppUser, object>>[] includes);
    }
}
