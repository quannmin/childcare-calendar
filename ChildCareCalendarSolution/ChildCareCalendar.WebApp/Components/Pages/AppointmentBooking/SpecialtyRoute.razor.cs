using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SpecialtyRoute
    {
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int userIdFromSession;

        private int? selectedSpecialtyId;
        private string? selectedSpecialtyName;
        private int? selectedServiceId;
        private string? selectedServiceName;

        [Inject] private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        [Inject] private NavigationManager Navigation { get; set; } = default!;

        [Inject] private IUserService UserService { get; set; } = default!;

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
            }
        }

        private void HandleSpecialtySelection(Speciality speciality)
        {
            selectedSpecialtyId = speciality.Id;
            selectedSpecialtyName = speciality.SpecialtyName;
            StateHasChanged();
        }
        private void HandleServiceSelection(Service service)
        {
            selectedServiceId = service.Id;
            selectedServiceName = service.ServiceName;
            StateHasChanged();
        }
        void GoBackToSpecialtySelection()
        {
            selectedSpecialtyId = null;
            selectedSpecialtyName = string.Empty;
        }

        void GoBackToServiceSelection()
        {
            selectedServiceId = null;
            selectedServiceName = string.Empty;
        }
    }
}