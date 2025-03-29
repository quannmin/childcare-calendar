using Microsoft.AspNetCore.Components;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChildCareCalendar.WebApp.Components.Pages.Doctors
{
    public partial class DoctorList
    {
        private List<AppUser> Doctors = new();
        private List<AppUser> FilteredDoctors = new();
        private List<AppUser> PagedDoctors = new();
        private List<Speciality> Specialties = new();

        private string searchTerm = string.Empty;
        private string selectedSpecialty = string.Empty;
        private string selectedGender = string.Empty;

        private int CurrentPage = 1;
        private int PageSize = 5;
        private int TotalPages = 1;

        [Inject] private IUserService UserService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadDoctors();
        }

        private async Task LoadDoctors()
        {
            Doctors = (await UserService.FindUsersAsync(
                d => d.Role.Equals("BacSi") && !d.IsDelete,
                d => d.Speciality
            )).ToList();

            Specialties = Doctors
                .Where(d => d.Speciality != null)
                .GroupBy(d => d.Speciality.Id)
                .Select(g => g.First().Speciality)
                .ToList();

            ApplyFilters();
        }

        // Khi giá trị thay đổi, tự động gọi ApplyFilters()
        private string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                ApplyFilters();
            }
        }

        private string SelectedSpecialty
        {
            get => selectedSpecialty;
            set
            {
                selectedSpecialty = value;
                ApplyFilters();
            }
        }

        private string SelectedGender
        {
            get => selectedGender;
            set
            {
                selectedGender = value;
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            int? specialtyFilter = string.IsNullOrEmpty(SelectedSpecialty) ? (int?)null : int.Parse(SelectedSpecialty);

            FilteredDoctors = Doctors
                .Where(d =>
                    (string.IsNullOrEmpty(SearchTerm) || d.FullName.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase)) &&
                    (!specialtyFilter.HasValue || d.Speciality?.Id == specialtyFilter.Value) &&
                    (string.IsNullOrEmpty(SelectedGender) || d.Gender == SelectedGender)
                ).ToList();

            TotalPages = (int)System.Math.Ceiling((double)FilteredDoctors.Count / PageSize);
            ChangePage(1);
        }

        private void ChangePage(int pageNumber)
        {
            Console.WriteLine($"Changing to page: {pageNumber}");
            if (pageNumber < 1 || pageNumber > TotalPages) return;

            CurrentPage = pageNumber;
            PagedDoctors = FilteredDoctors
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            StateHasChanged();
        }

        private void PreviousPage() => ChangePage(CurrentPage - 1);
        private void NextPage() => ChangePage(CurrentPage + 1);
    }
}
