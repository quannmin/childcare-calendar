using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Medicie;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Medicine
{
	public partial class Create
	{
		[SupplyParameterFromForm(FormName = "MedicineCreate")]
		private MedicineCreateViewModel CreateModel { get; set; } = new();

		[Inject]
		private IMedicineService MedicineService { get; set; } = default!;

		[Inject]
		private IMapper Mapper { get; set; } = default!;

		[Inject]
		private NavigationManager Navigation { get; set; } = default!;

		private string ErrorMessage = "";


		private async Task HandleCreate()
		{
			ErrorMessage = "";
			var listDuplicate = await MedicineService
				.FindMedicinesAsync(x => x.Name.Trim().ToLower().Equals(CreateModel.Name.Trim().ToLower()));
			if (listDuplicate.Count() > 0)
			{
				ErrorMessage = "Tên thuốc đã tồn tại.";
				return;
			}

			var newMedicine = Mapper.Map<Domain.Entities.Medicine>(CreateModel);
			await MedicineService.CreateMedineAsnync(newMedicine);
			Navigation.NavigateTo("/medicines");
		}
	}
}
