using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Org.BouncyCastle.Utilities;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectSpecialty
    {
        [Parameter]
        public EventCallback<Speciality> OnSpecialtySelected { get; set; }

        [Inject]
        ISpecialityService SpecialityService { get; set; } = default!;

        private List<Speciality> Specialties = new();

        protected override async Task OnInitializedAsync()
        {
            var result = await SpecialityService.FindSpecialitiesAsync(s => !s.IsDelete);
            Specialties = result.ToList();
        }
        private async void HandleSpecialtySelection(Speciality speciality)
        {
            Console.WriteLine("Selected Specialty: " + speciality.SpecialtyName);
            await OnSpecialtySelected.InvokeAsync(speciality);
        }

    }
}