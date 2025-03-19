using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChildCareCalendar.WebApp.Components.Pages.ChildrenRecords
{
    public partial class Edit
    {
        [Parameter] 
        public int id { get; set; }
        private ChildrenRecordEditViewModel ChildRecord { get; set; }

        [Inject]
        private IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private string ErrorMessage = "";

        protected override async Task OnInitializedAsync()
        {
            var cr = (await ChildrenRecordService.FindUsersAsync(c => c.Id.Equals(id))).FirstOrDefault();

            ChildRecord = Mapper.Map<ChildrenRecordEditViewModel>(cr);  
        }

        private async Task SaveChanges()
        {
            await ChildrenRecordService.UpdateChildrenRecordAsync(id, ChildRecord);
            Navigation.NavigateTo("/AccountDetail", forceLoad: true);
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/AccountDetail", forceLoad: true);
        }
    }
}