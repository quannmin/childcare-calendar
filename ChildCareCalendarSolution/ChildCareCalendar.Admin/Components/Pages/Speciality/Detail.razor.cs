using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace ChildCareCalendar.Admin.Components.Pages.Speciality
{
    public partial class Detail
    {
        [Parameter]
        public int id { get; set; }

        private SpecialityDetailViewModel DetailViewModel { get; set; } = new();

        [Inject]
        private ISpecialityService SpecialityService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            if (id != 0 && DetailViewModel.SpecialityId == 0)
            {
                var speciality = await SpecialityService.GetSpecialityByIdAsync(id);
                DetailViewModel = Mapper.Map<SpecialityDetailViewModel>(speciality);
            }
        }
    }
}
