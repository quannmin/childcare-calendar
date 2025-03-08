using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class DoctorRoute
    {
        private int? selectedDoctorId;
        private string? selectedDoctorName;
        private int? selectedSpecialtyId;
        private string? selectedSpecialtyName;
        private int? selectedServiceId;
        private string? selectedServiceName;

        private void HandleDoctorSelection(AppUser doctor)
        {
            selectedDoctorId = doctor.Id;
            selectedDoctorName = doctor.FullName;
            selectedSpecialtyId = doctor.Speciality.Id;
            selectedServiceName = doctor.Speciality.SpecialtyName;
            StateHasChanged();
        }
        private void HandleServiceSelection(Service service)
        {
            selectedServiceId = service.Id;
            selectedServiceName = service.ServiceName;
            StateHasChanged();
        }
        void GoBackToDoctorSelection()
        {
            selectedDoctorId = null;
            selectedDoctorName = string.Empty;
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