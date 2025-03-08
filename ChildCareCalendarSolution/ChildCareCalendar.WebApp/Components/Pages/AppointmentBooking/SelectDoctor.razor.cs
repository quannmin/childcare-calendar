using BCrypt.Net;
using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectDoctor
    {
        [Parameter]
        public EventCallback<AppUser> OnDoctorSelected { get; set; }

        private List<AppUser> Doctors = new()
        {
            new AppUser { Id = 2, Email = "doctor1@example.com", FullName = "Quân", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1,
            Speciality = new Speciality { Id = 1,  SpecialtyName = "Tai mũi họng" } },
            new AppUser { Id = 3, Email = "doctor1@example.com", FullName = "Lương", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
                        SpecialityId = 1,
            Speciality = new Speciality { Id = 1,  SpecialtyName = "Tai mũi họng" }},
            new AppUser { Id = 4, Email = "doctor1@example.com", FullName = "Quốc", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1,
            Speciality = new Speciality { Id = 1,  SpecialtyName = "Tai mũi họng" }},
            new AppUser { Id = 5, Email = "doctor1@example.com", FullName = "Qui", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1,
            Speciality = new Speciality { Id = 1,  SpecialtyName = "Tai mũi họng" }},
        };


        private async void HandleDoctorSelection(AppUser doctor)
        {
            Console.WriteLine("Selected Doctor: " + doctor.FullName);
            await OnDoctorSelected.InvokeAsync(doctor);
        }
    }
}