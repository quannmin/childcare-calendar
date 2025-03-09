using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Web;

namespace ChildCareCalendar.Admin.Components.Pages.Appointment
{
    public partial class Index
    {
        private List<AppointmentViewModel> Appointments = new();

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        IAppointmentService AppointmentService { get; set; }

        [SupplyParameterFromForm]
        private AppointmentSearchViewModel SearchData { get; set; } = new();
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        private int? idToDelete;
        private int CurrentPage = 1;
        private int PageSize = 10;
        private int TotalPages = 1;
        private int TotalItems = 0;

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);
            var page = queryParameters["page"];

            if (int.TryParse(page, out int pageNumber) && pageNumber > 0)
            {
                CurrentPage = pageNumber;
            }
            else
            {
                CurrentPage = 1;
            }

            await LoadAppointmentsAsync();
        }

        private async Task LoadAppointmentsAsync()
        {
            
            var (appointments, totalCount) = await AppointmentService.GetPagedAppointmentsAsync(CurrentPage, PageSize, SearchData.Keyword);
            Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete,

                            a => a.Parent, a => a.ChildrenRecord
                            ));
            TotalItems = totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        }

        //private async Task HandleSearch()
        //{
        //    var appointments = string.IsNullOrWhiteSpace(SearchData.Keyword)
        //        ? await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete, a => a.Parent, a => a.ChildrenRecord)
        //    : await AppointmentService.FindAppointmentsAsync(a => SearchData.Keyword.Equals(a.Parent.FullName)
        //                                                        || SearchData.Keyword.Equals(a.ChildrenRecord.FullName),
        //                                                        a => a.Parent, a => a.ChildrenRecord);

        //    Appointments = Mapper.Map<List<AppointmentViewModel>>(appointments);
        //    StateHasChanged();
        //}
        private async Task HandleSearch()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/appointments?page={CurrentPage}&search={SearchData.Keyword}", forceLoad: false);
            await LoadAppointmentsAsync();
        }
        private async Task ChangePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages && newPage != CurrentPage)
            {

                CurrentPage = newPage;
                await LoadAppointmentsAsync();
                Navigation.NavigateTo($"/appointments?page={CurrentPage}", forceLoad: false);


            }
        }

        private async void ConfirmDelete(int id)
        {
            idToDelete = id;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        private async Task DeleteAppointment()
        {
            if (idToDelete.HasValue)
            {
                await AppointmentService.DeleteAppointmentAsync(idToDelete.Value);
                idToDelete = null;
                await LoadAppointmentsAsync();
            }
            await JS.InvokeVoidAsync("hideDeleteModal");
        }
    }
}