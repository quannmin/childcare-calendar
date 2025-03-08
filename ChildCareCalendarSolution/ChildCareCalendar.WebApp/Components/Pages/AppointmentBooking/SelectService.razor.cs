using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectService
    {
        [Parameter] 
        public int SpecialtyId { get; set; }

        [Parameter]
        public EventCallback<Service> OnServiceSelected { get; set; }


        private List<Service> Services = new()
    {
        new Service
        {
            Id = 1,
            ServiceName = "Khám DV Khu E - Có BHYT",
            Price = 137.900,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        },
        new Service
        {
            Id = 2,
            ServiceName = "Khám DV Khu E - Không BHYT",
            Price = 180.000,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        },
        new Service
        {
            Id = 3,
            ServiceName = "Khám thường - Có BHYT",
            Price = 137.900,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        },
        new Service
        {
            Id = 4,
            ServiceName = "Khám thường - Không BHYT",
            Price = 180.000,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        }
    };

        private void ViewDetails(Service service)
        {
            Console.WriteLine($"Xem chi tiết: {service.ServiceName}");
        }

        private async void HandleServiceSelection(Service service)
        {
            Console.WriteLine("Selected Service: " + service.ServiceName);
            await OnServiceSelected.InvokeAsync(service);
        }
    }
}