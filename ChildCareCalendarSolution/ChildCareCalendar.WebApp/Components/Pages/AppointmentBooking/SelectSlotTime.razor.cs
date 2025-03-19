using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectSlotTime
    {
        [Parameter] public EventCallback<WorkHour> OnSlotSelected { get; set; }

        [Inject] private IWorkHourService WorkHourService { get; set; } = default!;

        private List<WorkHour> MorningWorkHours = new();
        private List<WorkHour> AfternoonWorkHours = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await WorkHourService.GetAllWorkHoursAsync();

                if (result == null || !result.Any())
                {
                    Console.WriteLine("❌ Không có dữ liệu WorkHour!");
                    return;
                }

                var allWorkHours = result.OrderBy(workHour => workHour.StartTime).ToList();

                MorningWorkHours = allWorkHours
                    .Where(w => w.StartTime >= TimeSpan.FromHours(7) && w.StartTime < TimeSpan.FromHours(12))
                    .ToList();

                AfternoonWorkHours = allWorkHours
                    .Where(w => w.StartTime >= TimeSpan.FromHours(12) && w.StartTime < TimeSpan.FromHours(17))
                    .ToList();

                Console.WriteLine($"✅ WorkHours Loaded: {allWorkHours.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi trong OnInitializedAsync: {ex.Message}");
            }
        }

        private async Task HandleSlotSelection(WorkHour workHour)
        {
            Console.WriteLine($"🔍 Selected Slot: {workHour.StartTime} - {workHour.EndTime}");
            await OnSlotSelected.InvokeAsync(workHour);
        }
    }
}
