using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Constants;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using System.Web;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserIndex
    {
        [Inject]
        private IUserService userService { get; set; }
        [Inject]
        private IMapper Mapper { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; }
        private List<UserViewModel> UserViewModels = new();
        private string Keyword = "";
        private int? userIdToDelete;

        private int CurrentPage = 1;
        private int PageSize = 10;
        private int TotalPages = 1;
        private int TotalItems = 0;

        private string GetRoleDisplayName(string role)
        {
            return Enum.TryParse(role, out SystemConstant.AccountsRole parsedRole)
                ? parsedRole.GetDisplayName()
                : role;
        }

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);
            var page = queryParameters["page"];

            if (int.TryParse(page, out int pageNumber) && pageNumber > 0)
            {
                CurrentPage = pageNumber;
            }
            else
            {
                CurrentPage = 1;
            }

            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            var (users, totalCount) = await userService.GetPagedUsersAsync(CurrentPage, PageSize, Keyword);
            UserViewModels = Mapper.Map<List<UserViewModel>>(users);
            TotalItems = totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        }

        private async Task SearchUsers()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/users/index?page={CurrentPage}&search={Keyword}", forceLoad: false);
            await LoadUsers();
        }
        private async Task ChangePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages && newPage != CurrentPage)
            {
                CurrentPage = newPage;
                Navigation.NavigateTo($"/users/index?page={CurrentPage}", forceLoad: false);
                await LoadUsers();
            }
        }


        private async void ConfirmDelete(int userId)
        {
            userIdToDelete = userId;
            await JS.InvokeVoidAsync("showDeleteModal");
        }


        private async Task DeleteUser()
        {
            if (userIdToDelete.HasValue)
            {
                await userService.DeleteUserAsync(userIdToDelete.Value);
                userIdToDelete = null;
                await LoadUsers();
            }
            await JS.InvokeVoidAsync("hideDeleteModal"); 
        }
    }
}
