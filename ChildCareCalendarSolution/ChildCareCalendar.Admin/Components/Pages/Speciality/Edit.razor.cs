using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
	public partial class Edit
	{
		[Parameter]
		public int id { get; set; }

		[SupplyParameterFromForm(FormName = "SpecialityUpdate")]
		private SpecialityEditViewModel EditModel { get; set; } = new();

		[Inject]
		private ISpecialityService SpecialityService { get; set; } = default!;

		[Inject]
		private IMapper Mapper { get; set; } = default!;

		[Inject]
		private NavigationManager Navigation { get; set; } = default!;

		protected override async Task OnInitializedAsync()
		{
			if (id != 0 && EditModel.SpecialityId == 0)
			{
				var speciality = await SpecialityService.GetSpecialityByIdAsync(id);
				EditModel = Mapper.Map<SpecialityEditViewModel>(speciality);
			}
		}

		private async Task HandleUpdate()
		{
			await SpecialityService.UpdateSpecialityAsync(Mapper.Map<Domain.Entities.Speciality>(EditModel));
			Navigation.NavigateTo("/specialities");
		}
	}
}
