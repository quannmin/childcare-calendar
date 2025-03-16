using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectService
    {
        [Parameter] 
        public int SpecialtyId { get; set; }

        [Parameter]
        public EventCallback<Service> OnServiceSelected { get; set; }

        [Inject]
        IServiceService ServiceService { get; set; } = default!;


        private List<Service> Services = new();


        protected override async Task OnInitializedAsync()
        {
            var result = await ServiceService.FindServicesAsync(s => s.SpecialityId.Equals(SpecialtyId) && !s.IsDelete);
            Services = result.ToList();
        }

        private async void HandleServiceSelection(Service service)
        {
            Console.WriteLine("Selected Service: " + service.ServiceName);
            await OnServiceSelected.InvokeAsync(service);
        }
    }
}