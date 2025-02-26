using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.ServiceVM;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Service
{
	public partial class Create
	{

		[Parameter]
		public int id { get; set; }

		[SupplyParameterFromForm(FormName = "ServiceCreate")]
		private ServiceCreateViewModel CreateModel { get; set; } = new();

		[Inject]
		private IServiceService ServiceService { get; set; } = default!;

		[Inject]
		private IMapper Mapper { get; set; } = default!;

		[Inject]
		private NavigationManager Navigation { get; set; } = default!;

		protected override void OnInitialized()
		{
			CreateModel.SpecialityId = id;
			CreateModel.CreatedAt = DateTime.UtcNow;
		}

		private async Task HandleCreate()
		{
			var newService = Mapper.Map<Domain.Entities.Service>(CreateModel);
			await ServiceService.AddServiceAsync(newService);
			Navigation.NavigateTo($"/specialities/detail/{id}");
		}
	}
}
