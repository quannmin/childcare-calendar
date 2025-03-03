using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

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
            Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete,
                a => a.Parent, a => a.ChildrenRecord
                ));
        }

        private async Task HandleSearch()
        {
            var appointments = string.IsNullOrWhiteSpace(SearchData.Keyword)
                ? await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete, a => a.Parent, a => a.ChildrenRecord)
            : await AppointmentService.FindAppointmentsAsync(a => SearchData.Keyword.Equals(a.Parent.FullName) 
                                                                || SearchData.Keyword.Equals(a.ChildrenRecord.FullName), 
                                                                a => a.Parent, a => a.ChildrenRecord);

            Appointments = Mapper.Map<List<AppointmentViewModel>>(appointments);
            StateHasChanged();
        }

        private async Task DeleteCategory(int id)
        {
            
        }
    }
}