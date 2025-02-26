using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

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

		protected override void OnInitialized()
		{
			CreateModel.CreatedAt = DateTime.UtcNow;
		}

		private async Task HandleCreate()
		{
			ErrorMessage = "";
			bool isDuplicate = await SpecialityService.CheckDuplicateSpecialtyNameAsync(CreateModel.SpecialtyName);
			if (isDuplicate)
			{
				ErrorMessage = "Tên chuyên khoa đã tồn tại.";
				return;
			}

			var newSpeciality = Mapper.Map<Domain.Entities.Speciality>(CreateModel);
			await SpecialityService.AddSpecialityAsync(newSpeciality);
			Navigation.NavigateTo("/specialities");
		}
	}
}
