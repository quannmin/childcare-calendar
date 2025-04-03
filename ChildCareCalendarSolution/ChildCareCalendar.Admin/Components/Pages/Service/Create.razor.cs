using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.ServiceVM;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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
        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        private string fullName = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            fullName = user.FindFirst("FullName")?.Value ?? string.Empty;
        }
        protected override void OnInitialized()
		{
			CreateModel.SpecialityId = id;
		}

		private async Task HandleCreate()
		{
			var newService = Mapper.Map<Domain.Entities.Service>(CreateModel);
			newService.CreatedBy = fullName;
			newService.CreatedAt = DateTime.UtcNow;
            await ServiceService.AddServiceAsync(newService);
			Navigation.NavigateTo($"/specialities/detail/{id}");
		}
	}
}
