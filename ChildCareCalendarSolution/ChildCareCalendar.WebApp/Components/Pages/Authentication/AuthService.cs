using BCrypt.Net;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.WebApp.Components.Pages.Authentication
{ 
        public class AuthService
        {
            private readonly IUserService _userService;
            private readonly ProtectedSessionStorage _sessionStorage;
            private readonly AuthenticationStateProvider _authenticationStateProvider;

            public AuthService(
                IUserService userService,
                ProtectedSessionStorage sessionStorage,
                AuthenticationStateProvider authenticationStateProvider)
            {
                _userService = userService;
                _sessionStorage = sessionStorage;
                _authenticationStateProvider = authenticationStateProvider;
            }

            public async Task<bool> LoginAsync(string email, string password)
            {
                // Validate user credentials
                var user = (await _userService.FindUsersAsync(u =>
                    u.Email == email.Trim() && !u.IsDelete)).FirstOrDefault();

                if (user == null)
                    return false;

                bool isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(
                    password,
                    user.Password,
                    HashType.SHA256);

                if (!isPasswordValid)
                    return false;

            // Store user information in session
            await _sessionStorage.SetAsync("userId", user.Id);
            await _sessionStorage.SetAsync("email", user.Email);
            await _sessionStorage.SetAsync("role", user.Role);
            await _sessionStorage.SetAsync("fullname", user.FullName);     


            // Force authentication state to refresh
            ((CustomAuthStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();

                return true;
            }

        public async Task<bool> LoginWithGoogleAsync(ClaimsPrincipal principal)
        {
            var email = principal.FindFirstValue(ClaimTypes.Email);
            var name = principal.FindFirstValue(ClaimTypes.Name);
            var picture = principal.FindFirstValue("picture");

            // Kiểm tra xem người dùng đã tồn tại trong hệ thống chưa
            var user = (await _userService.FindUsersAsync(u =>
                u.Email == email && !u.IsDelete)).FirstOrDefault();

            if (user == null)
            {
                // Tạo người dùng mới nếu chưa tồn tại
                user = new Domain.Entities.AppUser
                {
                    Email = email,
                    FullName = name,
                    Role = AccountsRole.PhuHuynh.ToString(), 
                    IsDelete = false,               
                };
                await _userService.AddUserAsync(user);
            }

            // Lưu thông tin vào session
            await _sessionStorage.SetAsync("userId", user.Id);
            await _sessionStorage.SetAsync("email", user.Email);
            await _sessionStorage.SetAsync("role", user.Role);
            await _sessionStorage.SetAsync("fullname", user.FullName);
            await _sessionStorage.SetAsync("avatar", picture ?? "");

            // Cập nhật trạng thái xác thực
            ((CustomAuthStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();

            return true;
        }

        public async Task LogoutAsync()
            {
            await _sessionStorage.DeleteAsync("userId");
            await _sessionStorage.DeleteAsync("email");
            await _sessionStorage.DeleteAsync("role");
            await _sessionStorage.DeleteAsync("fullname");
            ((CustomAuthStateProvider)_authenticationStateProvider).NotifyAuthenticationStateChanged();
            }
        }
}
