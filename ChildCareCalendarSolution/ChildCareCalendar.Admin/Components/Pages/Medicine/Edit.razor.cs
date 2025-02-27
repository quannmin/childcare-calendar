using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Medicie;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Medicine
{
	public partial class Edit
	{
		[Parameter]
		public int id { get; set; }

		[SupplyParameterFromForm(FormName = "MedicineUpdate")]
		private MedicineEditViewModel EditModel { get; set; } = new();

		[Inject]
		private IMedicineService MedicineService { get; set; } = default!;

		[Inject]
		private IMapper Mapper { get; set; } = default!;

		[Inject]
		private NavigationManager Navigation { get; set; } = default!;

		private string ErrorMessage = "";


		protected override async Task OnInitializedAsync()
		{
			if (id != 0 && EditModel.Id == 0)
			{
				var medicine = await MedicineService.FindMedicinesAsync(x => x.Id == id);
				EditModel = Mapper.Map<MedicineEditViewModel>(medicine.FirstOrDefault());
			}
		}

		private async Task HandleUpdate()
		{
			ErrorMessage = "";
			var listDuplicate = await MedicineService.FindMedicinesAsync(x =>
				x.Name.Trim().ToLower().Equals(EditModel.Name.Trim().ToLower()) &&
				x.Id != EditModel.Id);
			if (listDuplicate.Count() > 0)
			{
				ErrorMessage = "Tên thuốc đã tồn tại.";
				return;
			}
			await MedicineService.UpdateMedicineAsync(Mapper.Map<Domain.Entities.Medicine>(EditModel));
			Navigation.NavigateTo("/medicines");
		}
	}
}
