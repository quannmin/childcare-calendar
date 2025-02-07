using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<AppUser> _userRepository;

        public UserService(IRepository<AppUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUserAsync(AppUser user)
        {
            await _userRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);    
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            await _userRepository.UpdateAsync(user);    
        }
    }
}
