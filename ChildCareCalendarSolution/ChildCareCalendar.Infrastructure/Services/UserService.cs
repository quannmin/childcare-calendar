using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

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

        public async Task<AppUser> FindUserAsync(Expression<Func<AppUser, bool>> predicate, params Expression<Func<AppUser, object>>[] includes)
        {
            var user = await _userRepository.FindAsync(predicate, includes);
            return user.FirstOrDefault();
        }


        public async Task<IEnumerable<AppUser>> FindUsersAsync(Expression<Func<AppUser, bool>> predicate,
                                                        params Expression<Func<AppUser, object>>[] includes)
        {
            return await _userRepository.FindAsync(predicate, includes);
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            await _userRepository.UpdateAsync(user, user.Id);
        }

        public async Task<(IEnumerable<AppUser> users, int totalCount)> GetPagedUsersAsync(int pageIndex, int pageSize, string keyword = null)
        {
            Expression<Func<AppUser, bool>> filter = null;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.FullName.Contains(keyword) && !x.IsDelete;
            }
            else
            {
                filter = x => !x.IsDelete;
            }

            return await _userRepository.GetPagedAsync(pageIndex, pageSize, filter);
        }

    }
}
