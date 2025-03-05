using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.WorkHour;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.WorkHour
{
    public partial class Edit
	{
        [Parameter]
        public int id { get; set; }

        [SupplyParameterFromForm(FormName = "WorkHourUpdate")]
        private WorkHourEditViewModel EditModel { get; set; } = new();

        [Inject]
        private IWorkHourService WorkHourService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;
		[Inject]
		private IJSRuntime JS { get; set; } = default!;

		[Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private string ErrorMessage = "";

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JS.InvokeVoidAsync("flatpickr", "#startTime", new
				{
					enableTime = true,
					noCalendar = true,
					dateFormat = "H:i",
					time_24hr = true,
					allowInput = false
				});

				await JS.InvokeVoidAsync("flatpickr", "#endTime", new
				{
					enableTime = true,
					noCalendar = true,
					dateFormat = "H:i",
					time_24hr = true,
					allowInput = false
				});
			}
		}

		protected override async Task OnInitializedAsync()
        {
            if (id != 0 && EditModel.Id == 0)
            {
                var workHour = await WorkHourService.FindWorkHoursAsync(x => x.Id == id);
                EditModel = Mapper.Map<WorkHourEditViewModel>(workHour.FirstOrDefault());
            }
        }

        private async Task HandleUpdate()
        {
            ErrorMessage = "";

			if (EditModel.StartTime >= EditModel.EndTime)
			{
				ErrorMessage = "Giờ bắt đầu phải sớm hơn giờ kết thúc.";
				return;
			}

			var checkDuplicate = await WorkHourService.FindWorkHoursAsync(x =>
				!x.IsDelete &&
				x.StartTime == EditModel.StartTime &&
				x.EndTime == EditModel.EndTime
			);

			if (checkDuplicate.Any())
			{
				ErrorMessage = "Khung giờ này đã tồn tại.";
				return;
			}

			await WorkHourService.UpdateWorkHourAsync(Mapper.Map<Domain.Entities.WorkHour>(EditModel));
            Navigation.NavigateTo("/workhours");
        }
    }
}
