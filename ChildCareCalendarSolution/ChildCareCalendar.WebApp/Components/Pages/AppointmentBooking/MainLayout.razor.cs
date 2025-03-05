namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class MainLayout
    {
        private int? selectedSpecialtyId;

        private void HandleSpecialtySelection(int specialtyId)
        {
            Console.WriteLine($"Specialty Selected: {specialtyId}");
            selectedSpecialtyId = specialtyId;
        }
    }
}