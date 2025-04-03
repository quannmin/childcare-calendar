using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectChild
    {
        [Parameter]
        public int ParentId { get; set; }

        [Parameter]
        public EventCallback<ChildrenRecord> OnChildSelected { get; set; }

        [Inject]
        IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        NavigationManager Navigation { get; set; } = default!;

        private List<ChildrenRecord> ChildrenRecords = new();

        protected override async Task OnInitializedAsync()
        {
            var result = await ChildrenRecordService.FindChildrenRecordsAsync(c => c.Parent.Id.Equals(ParentId) && !c.IsDelete);
            ChildrenRecords = result.ToList();
        }

        private async void HandleChildSelection(ChildrenRecord child)
        {
            Console.WriteLine("Selected Child: " + child.FullName);
            await OnChildSelected.InvokeAsync(child);
        }

        private void NavigateToAccountDetail()
        {
            Navigation.NavigateTo("/AccountDetail", forceLoad: true);
        }
    }
}