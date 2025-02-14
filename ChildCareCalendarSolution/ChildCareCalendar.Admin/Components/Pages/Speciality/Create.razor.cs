using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
    public partial class Create
    {
        private string? ErrorMessage;

        [SupplyParameterFromForm]
        private SpecialityCreateViewModel CreateModel { get; set; }

        [Inject]
        private ISpecialityService SpecialityService { get; set; }

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }


        protected override void OnInitialized()
        {
            CreateModel ??= new();
        }

        private async Task HandleCreate()
        {
      
                var newSpeciality = Mapper.Map<Domain.Entities.Speciality>(CreateModel);
                await SpecialityService.AddSpecialityAsync(newSpeciality);
                Navigation.NavigateTo("/specialities");
        }
    }
}
