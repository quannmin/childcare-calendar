using AutoMapper;
using ChildCareCalendar.Domain.ViewModels;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
    public partial class Create
    {
        private string? ErrorMessage;
        private SpecialityCreateViewModel createModel { get; set; } = new();
        [Inject]
        private ISpecialityService SpecialityService { get; set; } = default!;
        private NavigationManager navigationManager { get; set; }
        private IMapper Mapper { get; set; } = default!;

        private async Task HandleCreate()
        {
            try
            {
                var newSpeciality = Mapper.Map<ChildCareCalendar.Domain.Entities.Speciality>(createModel);

                await SpecialityService.AddSpecialityAsync(newSpeciality);
                navigationManager.NavigateTo("/specialities");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
