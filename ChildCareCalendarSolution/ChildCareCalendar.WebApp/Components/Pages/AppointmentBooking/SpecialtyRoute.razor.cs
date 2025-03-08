using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SpecialtyRoute
    {
        private int? selectedSpecialtyId;
        private string? selectedSpecialtyName;
        private int? selectedServiceId;
        private string? selectedServiceName;

        private void HandleSpecialtySelection(Speciality speciality)
        {
            selectedSpecialtyId = speciality.Id;
            selectedSpecialtyName = speciality.SpecialtyName;
            StateHasChanged();
        }
        private void HandleServiceSelection(Service service)
        {
            selectedServiceId = service.Id;
            selectedServiceName = service.ServiceName;
            StateHasChanged();
        }
        void GoBackToSpecialtySelection()
        {
            selectedSpecialtyId = null;
            selectedSpecialtyName = string.Empty;
        }

        void GoBackToServiceSelection()
        {
            selectedServiceId = null;
            selectedServiceName = string.Empty;
        }
    }
}