using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
	public partial class Index
	{
		[Inject]
		private ISpecialityService SpecialityService { get; set; } = default!;
		[Inject]
		private IMapper Mapper { get; set; } = default!;
		private List<SpecialityViewModel> Specialities = new();
		[SupplyParameterFromForm]
		private SpecialitySearchViewModel SearchData { get; set; } = new();

		[Inject]
		private IJSRuntime JS { get; set; }

		private int? idToDelete;

		protected override async Task OnInitializedAsync()
		{
			await LoadSpecialities();
		}

		private async Task LoadSpecialities()
		{
			var specialities = await SpecialityService.FindSpecialitiesAsync(x => !x.IsDelete);
			Specialities = Mapper.Map<List<SpecialityViewModel>>(specialities);
			StateHasChanged();
		}

		private async Task HandleSearch()
		{
			var specialities = string.IsNullOrWhiteSpace(SearchData.Keyword)
				? await SpecialityService.FindSpecialitiesAsync(x => !x.IsDelete)
				: await SpecialityService.FindSpecialitiesAsync(x =>
					(x.SpecialtyName.ToLower().Contains(SearchData.Keyword.ToLower()) ||
					x.Description.ToLower().Contains(SearchData.Keyword.ToLower())) && 
					!x.IsDelete);

			Specialities = Mapper.Map<List<SpecialityViewModel>>(specialities);
			StateHasChanged();
		}

		private async void ConfirmDelete(int id)
		{
			idToDelete = id;
			await JS.InvokeVoidAsync("showDeleteModal");
		}

		private async Task DeleteSpeciality()
		{
			if (idToDelete.HasValue)
			{
				await SpecialityService.DeleteSpecialityAsync(idToDelete.Value);
				idToDelete = null;
				await LoadSpecialities();
			}
			await JS.InvokeVoidAsync("hideDeleteModal");
		}
	}
}