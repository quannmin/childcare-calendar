using ChildCareCalendar.Domain.Entities;
using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ChildCareCalendar.WebApp.Components
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public UserCreateLoginGGViewModel userCreateViewModel { get; set; } = new();
        private IMapper _mapper { get; set; } = default!;

        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("google-login")]
        public async Task<ActionResult> Google()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"),
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return LocalRedirect("/login?error=GoogleAuthFailed");

            var externalUser = result.Principal;

            // Lấy thông tin từ claims
            var emailClaim = externalUser.FindFirst(ClaimTypes.Email);
            var nameClaim = externalUser.FindFirst(ClaimTypes.Name);

            if (emailClaim == null)
                return LocalRedirect("/login?error=EmailNotProvided");

            var email = emailClaim.Value;
            var name = nameClaim?.Value ?? email;

            var userEntity = await _userService.FindUsersAsync(u => u.Email.Equals(email) && !u.IsDelete);
            var existedUser = userEntity.FirstOrDefault();

            if (existedUser == null)
            {
                userCreateViewModel.Email = email;
                userCreateViewModel.FullName = name;
                userCreateViewModel.Role = Utilities.Constants.SystemConstant.AccountsRole.PhuHuynh;

                AppUser user = _mapper.Map<AppUser>(userCreateViewModel);
                user.CreatedAt = DateTime.UtcNow;

                existedUser = await _userService.AddUser(user);
                if (existedUser == null)
                {
                    return LocalRedirect("/login?error=RegistrationFailed");
                }
            }

            // Tạo claims cho người dùng đã đăng nhập
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existedUser.Id.ToString()),
                new Claim(ClaimTypes.Name, existedUser.FullName),
                new Claim(ClaimTypes.Email, existedUser.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Đăng nhập người dùng
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            return LocalRedirect("/");
        }
    }
}