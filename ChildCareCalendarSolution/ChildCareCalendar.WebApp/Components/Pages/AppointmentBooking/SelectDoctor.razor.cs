using BCrypt.Net;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Web;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectDoctor
    {
        [Parameter]
        public EventCallback<AppUser> OnDoctorSelected { get; set; }

        [Inject]
        IUserService UserService { get; set; } = default!;

        [Inject]
        NavigationManager Navigation { get; set; } = default!;

        private List<AppUser> Doctors = new();
        private SearchModel SearchData = new();

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);

            // Lấy từ khóa tìm kiếm từ URL
            SearchData.Keyword = queryParameters["search"] ?? "";

            await LoadDoctorsAsync(SearchData.Keyword);
        }

        private async Task LoadDoctorsAsync(string? filter = null)
        {
            // Lấy toàn bộ danh sách bác sĩ từ database
            var result = await UserService.FindUsersAsync(
                d => d.Role.Equals("BacSi") && !d.IsDelete,
                d => d.Speciality
            );

            // Lọc danh sách trên client để tránh lỗi LINQ
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower(); // Chuyển về chữ thường để so sánh
                result = result.Where(d => d.FullName.ToLower().Contains(filter));
            }

            Doctors = result.ToList();
        }

        private async Task HandleSearch()
        {
            Navigation.NavigateTo($"/Appointment/Doctor?search={SearchData.Keyword}", forceLoad: true);
            await LoadDoctorsAsync(SearchData.Keyword);
        }

        private async void HandleDoctorSelection(AppUser doctor)
        {
            Console.WriteLine("Selected Doctor: " + doctor.FullName);
            await OnDoctorSelected.InvokeAsync(doctor);
        }

        private class SearchModel
        {
            public string Keyword { get; set; } = "";
        }
    }
}