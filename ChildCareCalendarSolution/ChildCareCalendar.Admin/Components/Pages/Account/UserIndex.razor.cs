using AutoMapper;
using ChildCareCalendar.Admin.Extensions;
using ChildCareCalendar.Domain.ViewModels;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Constants;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserIndex
    {
        [Inject]
        private IUserService userService { get; set; }
        [Inject]
        private IMapper Mapper { get; set; }
        private List<UserViewModel> UserViewModels = new();
        private string Keyword = "";
        private NavigationManager navigationManager { get; set; }

        private string GetRoleDisplayName(string role)
        {
            if (Enum.TryParse(role, out SystemConstant.AccountsRole parsedRole))
            {
                return parsedRole.GetDisplayName();
            }

            return role;
        }

        protected override async Task OnInitializedAsync()
        {
            UserViewModels = Mapper.Map<List<UserViewModel>>(await userService.GetAllUsersAsync());
        }

        private async Task SearchUsers()
        {
            var users = string.IsNullOrEmpty(Keyword)
                ? await userService.GetAllUsersAsync() : await userService.GetAllUsersByNameAsync(Keyword);

            UserViewModels = Mapper.Map<List<UserViewModel>>(users);
        }

        private async Task DeleteCategory(int id)
        {
            await userService.DeleteUserAsync(id);
            var users = await userService.GetAllUsersAsync();
            UserViewModels = Mapper.Map<List<UserViewModel>>(users);
        }
    }
}
