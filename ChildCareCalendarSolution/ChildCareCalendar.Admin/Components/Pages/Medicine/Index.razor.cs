using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Medicie;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Medicine
{
    public partial class Index
    {
        [Inject]
        private IMedicineService MedicineService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;
        private List<MedicineViewModel> Medicines = new();
        [SupplyParameterFromForm]
        private MedicineSearchViewModel SearchData { get; set; } = new();

        [Inject]
        private IJSRuntime JS { get; set; }

        private int? idToDelete;

        protected override async Task OnInitializedAsync()
        {
            await LoadMedicines();
        }

        private async Task LoadMedicines()
        {
            var medicines = await MedicineService.FindMedicinesAsync(x => !x.IsDelete);
            Medicines = Mapper.Map<List<MedicineViewModel>>(medicines);
            StateHasChanged();
        }

        private async Task HandleSearch()
        {
            var medicines = string.IsNullOrWhiteSpace(SearchData.Keyword)
                ? await MedicineService.FindMedicinesAsync(x => !x.IsDelete)
                : await MedicineService.FindMedicinesAsync(x =>
                    x.Name.ToLower().Contains(SearchData.Keyword.ToLower()) &&
                    !x.IsDelete);

            Medicines = Mapper.Map<List<MedicineViewModel>>(medicines);
            StateHasChanged();
        }

        private async void ConfirmDelete(int id)
        {
            idToDelete = id;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        private async Task DeleteMedicine()
        {
            if (idToDelete.HasValue)
            {
                await MedicineService.DeleteMedicineAsync(idToDelete.Value);
                idToDelete = null;
                await LoadMedicines();
            }
            await JS.InvokeVoidAsync("hideDeleteModal");
        }
    }
}
