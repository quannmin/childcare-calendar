using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
	public partial class Create
	{
		[SupplyParameterFromForm(FormName = "SpecialityCreate")]
		private SpecialityCreateViewModel CreateModel { get; set; } = new();

		[Inject]
		private ISpecialityService SpecialityService { get; set; } = default!;

		[Inject]
		private IMapper Mapper { get; set; } = default!;

		[Inject]
		private NavigationManager Navigation { get; set; } = default!;

		private string ErrorMessage = "";

		[Inject]
		AuthenticationStateProvider AuthStateProvider {  get; set; } = default!;

		private string fullName = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			var authState = await AuthStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			fullName = user.FindFirst("FullName")?.Value ?? string.Empty;
		}

		protected override void OnInitialized()
		{
			CreateModel.CreatedAt = DateTime.UtcNow;
		}

		
		private async Task HandleCreate()
		{
			ErrorMessage = "";
			var listDuplicate = await SpecialityService
				.FindSpecialitiesAsync(x => x.SpecialtyName.Trim().ToLower().Equals(CreateModel.SpecialtyName.Trim().ToLower()));
			if (listDuplicate.Count() > 0)
			{
				ErrorMessage = "Tên chuyên khoa đã tồn tại.";
				return;
			}

			var newSpeciality = Mapper.Map<Domain.Entities.Speciality>(CreateModel);
			newSpeciality.CreatedBy = fullName;

			await SpecialityService.AddSpecialityAsync(newSpeciality);
			Navigation.NavigateTo("/specialities");
		}
	}
}
