using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.WorkHour;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.WorkHour
{
	public partial class Create
	{
		[SupplyParameterFromForm(FormName = "WorkHourCreate")]
		private WorkHourCreateViewModel CreateModel { get; set; } = new();

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

		private async Task HandleCreate()
		{
			ErrorMessage = "";

			if (CreateModel.StartTime >= CreateModel.EndTime)
			{
				ErrorMessage = "Giờ bắt đầu phải sớm hơn giờ kết thúc.";
				return;
			}

			var checkDuplicate = await WorkHourService.FindWorkHoursAsync(x =>
				!x.IsDelete &&
				x.StartTime == CreateModel.StartTime &&
				x.EndTime == CreateModel.EndTime
			);

			if (checkDuplicate.Any())
			{
				ErrorMessage = "Khung giờ này đã tồn tại.";
				return;
			}

			var newWorkHour = Mapper.Map<Domain.Entities.WorkHour>(CreateModel);
			await WorkHourService.AddWorkHourAsync(newWorkHour);
			Navigation.NavigateTo("/workhours");
		}
	}
}
