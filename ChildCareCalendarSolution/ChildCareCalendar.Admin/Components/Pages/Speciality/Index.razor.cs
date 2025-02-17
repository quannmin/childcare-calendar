using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
    public partial class Index
    {
        [Inject]
        private ISpecialityService SpecialityService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;
        private List<SpecialityViewModel> Specialities = new();
        [SupplyParameterFromForm]
        private SpecialitySearchViewModel SearchData { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            await LoadSpecialities();
        }

        private async Task LoadSpecialities()
        {
            var specialities = await SpecialityService.GetAllSpecialitiesAsync();
            Specialities = Mapper.Map<List<SpecialityViewModel>>(specialities);
            StateHasChanged();
        }

        private async Task HandleSearch()
        {
            var specialities = string.IsNullOrWhiteSpace(SearchData.Keyword)
                ? await SpecialityService.GetAllSpecialitiesAsync()
                : await SpecialityService.FindSpeciallityAsync(SearchData.Keyword);

            Specialities = Mapper.Map<List<SpecialityViewModel>>(specialities);
            StateHasChanged();
        }

        private async Task DeleteCategory(int id)
        {
            await SpecialityService.DeleteSpecialityAsync(id);
            var specialities = await SpecialityService.GetAllSpecialitiesAsync();
            Specialities = Mapper.Map<List<SpecialityViewModel>>(specialities);
        }
    }
}