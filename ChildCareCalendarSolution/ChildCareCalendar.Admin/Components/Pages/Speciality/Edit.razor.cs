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

		private string ErrorMessage = "";


		protected override async Task OnInitializedAsync()
		{
			if (id != 0 && EditModel.Id == 0)
			{
				var speciality = await SpecialityService.FindSpecialitiesAsync(x => x.Id == id);
				EditModel = Mapper.Map<SpecialityEditViewModel>(speciality.FirstOrDefault());
			}
		}

		private async Task HandleUpdate()
		{
			ErrorMessage = "";
			var listDuplicate = await SpecialityService.FindSpecialitiesAsync(x =>
				x.SpecialtyName.Trim().ToLower().Equals(EditModel.SpecialtyName.Trim().ToLower()) &&
				x.Id != EditModel.Id);
			if (listDuplicate.Count() > 0)
			{
				ErrorMessage = "Tên chuyên khoa đã tồn tại.";
				return;
			}
			var updateItem = await SpecialityService.FindSpecialitiesAsync(x => x.Id == EditModel.Id);
			await SpecialityService.UpdateSpecialityAsync(Mapper.Map(EditModel, updateItem.FirstOrDefault()));
			Navigation.NavigateTo("/specialities");
		}
	}
}
