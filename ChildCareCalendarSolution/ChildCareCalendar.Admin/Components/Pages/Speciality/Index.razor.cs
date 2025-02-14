using AutoMapper;
using ChildCareCalendar.Domain.ViewModels;
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
        private List<SpecialityViewModel> Categories = new();
        private string Keyword = "";
        private NavigationManager navigationManager { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Categories = Mapper.Map<List<SpecialityViewModel>>(await SpecialityService.GetAllSpecialitiesAsync());
        }

        private async Task SearchCategories()
        {
            var specialities = string.IsNullOrWhiteSpace(Keyword)
                ? await SpecialityService.GetAllSpecialitiesAsync()
                : await SpecialityService.GetSpecialityByNameAsync(Keyword);

            Categories = Mapper.Map<List<SpecialityViewModel>>(specialities);
        }

        private async Task DeleteCategory(int id)
        {
            await SpecialityService.DeleteSpecialityAsync(id);
            var specialities = await SpecialityService.GetAllSpecialitiesAsync();
            Categories = Mapper.Map<List<SpecialityViewModel>>(specialities);
        }
    }
}
