using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class MainLayout
    {
        private int? selectedSpecialtyId;
        private string? selectedSpecialtyName;

        private void HandleSpecialtySelection(Speciality speciality)
        {
            selectedSpecialtyId = speciality.Id;
            selectedSpecialtyName = speciality.SpecialtyName;
            StateHasChanged();
        }
    }
}