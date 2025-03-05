using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectService
    {
        private List<Service> Services = new()
    {
        new Service
        {
            ServiceName = "Khám DV Khu E - Có BHYT",
            Price = 137.900,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        },
        new Service
        {
            ServiceName = "Khám DV Khu E - Không BHYT",
            Price = 180.000,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        },
        new Service
        {
            ServiceName = "Khám thường - Có BHYT",
            Price = 137.900,
            SpecialityId = 1,
            Description = "Thanh toán tại Bệnh viện",
            CreatedAt = DateTime.Now,
        },
        new Service
        {
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

        private void BookNow(Service service)
        {
            Console.WriteLine($"Đặt khám: {service.ServiceName}");
        }
    }
}