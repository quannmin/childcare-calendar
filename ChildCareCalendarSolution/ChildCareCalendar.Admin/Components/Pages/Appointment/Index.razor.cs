using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace ChildCareCalendar.Admin.Components.Pages.Appointment
{
    public partial class Index
    {
        private List<AppointmentViewModel> Appointments = new();

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        IAppointmentService AppointmentService { get; set; }

        [SupplyParameterFromForm]
        private AppointmentSearchViewModel SearchData { get; set; } = new();
        [Inject]
        private NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.GetAllAppointmentsAsync(
                a => a.Parent, a => a.ChildrenRecord, a => a.Service
                ));
        }

        private async Task HandleSearch()
        {
            var appointments = string.IsNullOrWhiteSpace(SearchData.Keyword)
                ? await AppointmentService.GetAllAppointmentsAsync(a => a.Parent, a => a.ChildrenRecord, a => a.Service)
                : await AppointmentService.FindAppointmentAsync(SearchData.Keyword);

            Appointments = Mapper.Map<List<AppointmentViewModel>>(appointments);
            StateHasChanged();
        }

        private async Task DeleteCategory(int id)
        {
            
        }
    }
}