using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Org.BouncyCastle.Utilities;
using System.Web;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectSpecialty
    {
        [Parameter]
        public EventCallback<Speciality> OnSpecialtySelected { get; set; }

        [Inject]
        ISpecialityService SpecialityService { get; set; } = default!;

        [Inject]
        NavigationManager Navigation { get; set; } = default!;

        private List<Speciality> Specialties = new();
        private SearchModel SearchData = new();

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);

            SearchData.Keyword = queryParameters["search"] ?? "";

            await LoadSpecialtyAsync(SearchData.Keyword);
        }

        private async Task LoadSpecialtyAsync(string? filter = null)
        {
            // Lấy toàn bộ danh sách specialty từ database
            var result = await SpecialityService.FindSpecialitiesAsync(s => !s.IsDelete);

            // Lọc danh sách trên client để tránh lỗi LINQ
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower(); // Chuyển về chữ thường để so sánh
                result = result.Where(d => d.SpecialtyName.ToLower().Contains(filter));
            }

            Specialties = result.ToList();
        }
        private async void HandleSpecialtySelection(Speciality speciality)
        {
            Console.WriteLine("Selected Specialty: " + speciality.SpecialtyName);
            await OnSpecialtySelected.InvokeAsync(speciality);
        }

        private async Task HandleSearch()
        {
            Navigation.NavigateTo($"/Appointment/Specialty?search={SearchData.Keyword}", forceLoad: true);
            await LoadSpecialtyAsync(SearchData.Keyword);
        }

        private class SearchModel
        {
            public string Keyword { get; set; } = "";
        }
    }
}