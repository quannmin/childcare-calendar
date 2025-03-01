using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.WorkHour;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.WorkHour
{
	public partial class Index
	{
		[Inject]
		private IWorkHourService WorkHourService { get; set; } = default!;
		[Inject]
		private IMapper Mapper { get; set; } = default!;
		private List<WorkHourViewModel> WorkHours = new();

		[Inject]
		private IJSRuntime JS { get; set; }

		private int? idToDelete;

		protected override async Task OnInitializedAsync()
		{
			await LoadWorkHours();
		}

		private async Task LoadWorkHours()
		{
			var workHours = await WorkHourService.FindWorkHoursAsync(x => !x.IsDelete);
			WorkHours = Mapper.Map<List<WorkHourViewModel>>(workHours);
			StateHasChanged();
		}

		private async void ConfirmDelete(int id)
		{
			idToDelete = id;
			await JS.InvokeVoidAsync("showDeleteModal");
		}

		private async Task DeleteWorkHour()
		{
			if (idToDelete.HasValue)
			{
				await WorkHourService.DeleteWorkHourAsync(idToDelete.Value);
				idToDelete = null;
				await LoadWorkHours();
			}
			await JS.InvokeVoidAsync("hideDeleteModal");
		}
	}
}
