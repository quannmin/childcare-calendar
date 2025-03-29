using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Service;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
	public partial class Detail
    {
        [Parameter]
        public int id { get; set; }

        private SpecialityDetailViewModel DetailViewModel { get; set; } = new();
        private List<ServiceViewModel> ServiceViewModel { get; set; } = new();

        [Inject]
        private ISpecialityService SpecialityService { get; set; } = default!;
		[Inject]
		private IServiceService ServiceService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

		[Inject]
		private IJSRuntime JS { get; set; } = default!;

		private int? idToDelete;

		private int CurrentPage = 1;
		private int PageSize = 10;
		private int TotalPages = 1;
		private int TotalItems = 0;

		protected override async Task OnInitializedAsync()
        {
			await LoadData();

		}

        private async Task LoadData()
        {
			if (id != 0 && DetailViewModel.Id == 0)
			{
				var speciality = await SpecialityService.FindSpecialitiesAsync(x => x.Id == id);
				if (speciality.Any())
				{
					var firstSpeciality = speciality.FirstOrDefault();
					DetailViewModel = Mapper.Map<SpecialityDetailViewModel>(firstSpeciality);
				}

				var service = await ServiceService.FindServicesAsync(x => x.SpecialityId == id && !x.IsDelete);
				if (service.Any()) 
				{
					ServiceViewModel = Mapper.Map<List<ServiceViewModel>>(service);
				}
				StateHasChanged();
			}
		}

		private async void ConfirmDelete(int id)
		{
			idToDelete = id;
			await JS.InvokeVoidAsync("showDeleteModal");
		}

		private async Task DeleteService()
		{
			if (idToDelete.HasValue)
			{
				await ServiceService.DeleteServiceAsync(idToDelete.Value);
				idToDelete = null;
				await LoadData();
			}
			await JS.InvokeVoidAsync("hideDeleteModal");
		}
	}
}
