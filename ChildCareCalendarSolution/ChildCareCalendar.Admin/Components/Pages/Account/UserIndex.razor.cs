using AutoMapper;
using ChildCareCalendar.Admin.Extensions;
using ChildCareCalendar.Domain.ViewModels;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserIndex
    {
        [Inject]
        private IUserService userService { get; set; }
        [Inject]
        private IMapper Mapper { get; set; }
        [Inject]
        private IJSRuntime JS { get; set; }
        private List<UserViewModel> UserViewModels = new();
        private string Keyword = "";
        private int? userIdToDelete;

        private string GetRoleDisplayName(string role)
        {
            return Enum.TryParse(role, out SystemConstant.AccountsRole parsedRole)
                ? parsedRole.GetDisplayName()
                : role;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            UserViewModels = Mapper.Map<List<UserViewModel>>(await userService.GetAllUsersAsync());
        }

        private async Task SearchUsers()
        {
            var users = string.IsNullOrEmpty(Keyword)
                ? await userService.GetAllUsersAsync()
                : await userService.GetAllUsersByNameAsync(Keyword);

            UserViewModels = Mapper.Map<List<UserViewModel>>(users);
        }

        private async void ConfirmDelete(int userId)
        {
            userIdToDelete = userId;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        //private void ConfirmDelete(int userId)
        //{
        //    userIdToDelete = userId;
        //    JS.InvokeVoidAsync("showDeleteModal");
        //}

        private async Task DeleteUser()
        {
            if (userIdToDelete.HasValue)
            {
                await userService.DeleteUserAsync(userIdToDelete.Value);
                userIdToDelete = null;
                await LoadUsers();
            }
            await JS.InvokeVoidAsync("hideDeleteModal"); // Ẩn modal sau khi xóa
        }
    }
}
