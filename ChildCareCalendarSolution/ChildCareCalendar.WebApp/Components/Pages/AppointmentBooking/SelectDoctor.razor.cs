using BCrypt.Net;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectDoctor
    {
        [Parameter]
        public EventCallback<AppUser> OnDoctorSelected { get; set; }

        [Inject]
        IUserService UserService { get; set; } = default!;

        private List<AppUser> Doctors = new();

        protected override async Task OnInitializedAsync()
        {
            
            var result = await UserService.FindUsersAsync(d => d.Role.Equals("BacSi") && !d.IsDelete, d => d.Speciality);
            Doctors = result.ToList();
        }

        private async void HandleDoctorSelection(AppUser doctor)
        {
            Console.WriteLine("Selected Doctor: " + doctor.FullName);
            await OnDoctorSelected.InvokeAsync(doctor);
        }
    }
}