using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Web;

namespace ChildCareCalendar.Admin.Components.DoctorPages.Appointment
{
    public partial class Index
    {
        private List<AppointmentViewModel> Appointments = new();

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        IAppointmentService AppointmentService { get; set; }

        [Inject]
        IUserService UserService { get; set; }

        private int? DoctorId = 3;

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
            //var user = await UserService.GetCurrentUserAsync();         
         
            //if (user != null && user.Role == "Bacsi")
            //{
            //    DoctorId = user.DoctorId;
            //}
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
            var search = queryParameters["search"];
            if (!string.IsNullOrEmpty(search))
            {
                SearchData.Keyword = search;
            }
            await LoadAppointmentsAsync();
        }

        private async Task LoadAppointmentsAsync()
        {
            if (DoctorId.HasValue)
            {
                var (appointments, totalCount) = await AppointmentService.GetPagedAppointmentsByDoctorIdAsync(
                    DoctorId.Value, CurrentPage, PageSize, SearchData.Keyword);

                Appointments = Mapper.Map<List<AppointmentViewModel>>(appointments);
                TotalItems = totalCount;
                TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);      
            }
            else
            {
             
                Appointments = new List<AppointmentViewModel>();
                TotalItems = 0;
                TotalPages = 1;
            }
        }

        private async Task HandleSearch()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/appointmentsfordoctor?page={CurrentPage}&search={SearchData.Keyword}", forceLoad: false);
            await LoadAppointmentsAsync();
        }
        private async Task ChangePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages && newPage != CurrentPage)
            {

                CurrentPage = newPage;
                await LoadAppointmentsAsync();
                Navigation.NavigateTo($"/appointmentsfordoctor?page={CurrentPage}", forceLoad: false);


            }
        }

    }
}