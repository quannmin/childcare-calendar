using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChildCareCalendar.WebApp.Components.Pages.ChildrenRecords
{
    public partial class Create
    {
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int userIdFromSession;

        [SupplyParameterFromForm(FormName = "ChildrenRecordCreate")]
        private ChildrenRecordCreateViewModel CreateModel { get; set; } = new();

        [Inject]
        private IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        private string ErrorMessage = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var userIdResult = await SessionStorage.GetAsync<int>("userId");
                if (userIdResult.Success)
                {
                    userIdFromSession = userIdResult.Value;

                    Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(userIdFromSession)))?.FirstOrDefault();
                    if (Parent != null && Parent.Role.Equals("PhuHuynh"))
                    {
                        IsAuthenticated = true;
                    }
                    else
                    {
                        Navigation.NavigateTo("/Login", forceLoad: true);
                    }
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine("Không lấy được dữ liệu userId từ session.");
                    Navigation.NavigateTo("/Login", forceLoad: true);
                }
            }
        }

        private async Task HandleCreate()
        {
            try
            {
                var createModel = Mapper.Map<ChildrenRecord>(CreateModel);
                createModel.UserId = Parent.Id;
                createModel.CreatedAt = DateTime.Now;
                await ChildrenRecordService.AddChildrenRecordAsync(createModel);
                Navigation.NavigateTo("/AccountDetail", forceLoad: true);

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }
        }
    }
}