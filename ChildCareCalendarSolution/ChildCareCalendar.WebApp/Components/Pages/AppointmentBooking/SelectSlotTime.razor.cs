using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectSlotTime
    {
        [Parameter] public EventCallback<WorkHour> OnSlotSelected { get; set; }

        [Parameter] public int DoctorId { get; set; }
        [Parameter] public DateTime? SelectedDate { get; set; }

        [Inject] public IScheduleService ScheduleService { get; set; } = default!;

        private List<Schedule> MorningSlots = new();
        private List<Schedule> AfternoonSlots = new();

        protected override async Task OnParametersSetAsync()
        {
            if (SelectedDate.HasValue)
            {
                var selectedDate = SelectedDate.Value.Date;

                var all = (await ScheduleService.FindSchedulesAsync(
                    s => s.UserId == DoctorId &&
                         s.WorkDay.HasValue &&
                         s.WorkDay.Value.Date == selectedDate,
                    s => s.WorkHour))
                    .Where(s => s.WorkHour != null)
                    .OrderBy(s => s.WorkHour!.StartTime)
                    .ToList();

                MorningSlots = all.Where(w => w.WorkHour!.StartTime < TimeSpan.FromHours(12)).ToList();
                AfternoonSlots = all.Where(w => w.WorkHour!.StartTime >= TimeSpan.FromHours(12)).ToList();
            }
        }

        private bool IsSlotDisabled(Schedule s)
        {
            bool isToday = SelectedDate?.Date == DateTime.Today;
            bool isPastTime = isToday && s.WorkHour!.StartTime <= DateTime.Now.TimeOfDay;

            return s.IsOccupied || isPastTime;
        }

        private async Task HandleSlotSelection(Schedule slot)
        {
            if (slot.WorkHour != null)
            {
                await OnSlotSelected.InvokeAsync(slot.WorkHour);
            }
        }
    }
}