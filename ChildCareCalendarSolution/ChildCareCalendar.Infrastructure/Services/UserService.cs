using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

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
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _userRepository.AddAsync(user);
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                user.IsDelete = true;
                await _userRepository.UpdateAsync(user, user.Id);
                id = user.Id;
            }
            return id > 0 ? id : 0;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersByNameAsync(string name)
        {
            return await _userRepository.FindAsync(x => x.FullName.ToLower().Equals(name.ToLower()));
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            await _userRepository.UpdateAsync(user, user.Id);
        }
    }
}
