using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectSpecialty
    {
        [Parameter]
        public EventCallback<int> OnSpecialtySelected { get; set; }

        private List<Speciality> Specialties = new()
    {
        new Speciality {SpecialtyName = "CAN THIỆP MẠCH MÁU - U GAN", Description = "", CreatedAt = DateTime.Now },
        new Speciality {SpecialtyName = "CAN THIỆP TIM MẠCH - DSA", Description = "", CreatedAt = DateTime.Now },
        new Speciality {SpecialtyName = "CƠ XƯƠNG KHỚP", Description = "(Triệu chứng: Đau các khớp tay - chân / Đau lưng / Trật khớp / Bong gân / Gãy xương...)", CreatedAt = DateTime.Now },
        new Speciality {SpecialtyName = "DA LIỄU", Description = "(Triệu chứng: Ngứa ngoài da / Da tróc vảy / Nổi mẩn đỏ / Sạm da / Mụn / Nấm tóc...)", CreatedAt = DateTime.Now }
    };

        private async void HandleSpecialtySelection(int specialtyId)
        {
            Console.WriteLine($"Selected Specialty ID: {specialtyId}");
            await OnSpecialtySelected.InvokeAsync(specialtyId);

        }
    }
}