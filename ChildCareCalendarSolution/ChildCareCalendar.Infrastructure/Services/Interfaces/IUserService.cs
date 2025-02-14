using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<IEnumerable<AppUser>> GetAllUsersByNameAsync(string name);
        Task AddUserAsync(AppUser user);
        Task UpdateUserAsync(AppUser user);
        Task DeleteUserAsync(int id);

    }
}
