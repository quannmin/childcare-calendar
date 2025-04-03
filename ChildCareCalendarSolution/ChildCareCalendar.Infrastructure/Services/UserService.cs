using BCrypt.Net;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Constants;
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
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, HashType.SHA256);
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
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, HashType.SHA256);
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
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var users = await _userRepository.FindAsync(
                u => u.Email != null && u.Email.ToLower() == email.ToLower().Trim()
            );

            return users.FirstOrDefault();
        }

        public async Task<AppUser> AddUser(AppUser user)
        {
            return await _userRepository.Add(user);
        }

        public async Task<List<NewUsersByRoleViewModel>> GetDailyNewUsersByRoleAsync(int days)
        {
            // Tính ngày bắt đầu (30 ngày trước đến hiện tại)
            DateTime endDate = DateTime.Now.Date.AddDays(1); // Đến cuối ngày hôm nay
            DateTime startDate = endDate.AddDays(-days);

            // Lấy tất cả người dùng được tạo trong khoảng thời gian
            var users = await _userRepository.FindAsync(
                u => u.CreatedAt >= startDate &&
                     u.CreatedAt < endDate &&
                     !u.IsDelete);

            // Nhóm người dùng theo ngày và vai trò
            var result = Enumerable.Range(0, days)
                .Select(dayOffset => {
                    DateTime currentDate = startDate.AddDays(dayOffset);
                    DateOnly a = DateOnly.FromDateTime(currentDate);
                    var usersOnThisDay = users.Where(u => DateOnly.FromDateTime(u.CreatedAt) == DateOnly.FromDateTime(currentDate));

                    return new NewUsersByRoleViewModel
                    {
                        Date = currentDate,
                        ParentCount = usersOnThisDay.Count(u => u.Role.Equals("PhuHuynh")),
                        DoctorCount = usersOnThisDay.Count(u => u.Role.Equals("BacSi"))
                    };
                })
                .ToList();

            return result;
        }
    }
}
