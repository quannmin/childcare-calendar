using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.ServiceVM;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Service
{
    public partial class Edit
	{

		[Parameter]
		public int specialityId { get; set; }
        [Parameter]
        public int serviceId { get; set; }

        [SupplyParameterFromForm(FormName = "ServiceUpdate")]
		private ServiceEditViewModel EditModel { get; set; } = new();

		[Inject]
		private IServiceService ServiceService { get; set; } = default!;

		[Inject]
		private IMapper Mapper { get; set; } = default!;

		[Inject]
		private NavigationManager Navigation { get; set; } = default!;

		protected override void OnInitialized()
		{
			EditModel.SpecialityId = specialityId;
		}

		protected override async Task OnInitializedAsync()
		{
			if (serviceId != 0 && EditModel.Id == 0)
			{
				var service = await ServiceService.FindServicesAsync(x => x.Id == serviceId);
				EditModel = Mapper.Map<ServiceEditViewModel>(service.FirstOrDefault());
			}
		}

		private async Task HandleUpdate()
		{
			await ServiceService.UpdateServiceAsync(Mapper.Map<Domain.Entities.Service>(EditModel));
			Navigation.NavigateTo($"/specialities/detail/{specialityId}");
		}
	}
}
