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

        [SupplyParameterFromForm (FormName = "SearchAppointment")]
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

            if (int.TryParse(queryParameters["page"], out int pageNumber) && pageNumber > 0)
            {
                CurrentPage = pageNumber;
            }
            else
            {
                CurrentPage = 1;
            }

            // Lấy từ khóa tìm kiếm từ URL khi trang tải lại
            SearchData.Keyword = queryParameters["search"] ?? "";

            await LoadAppointmentsAsync();
        }


        private async Task LoadAppointmentsAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);

            // Lấy tham số search từ URL
            var searchKeyword = queryParameters["search"];
            SearchData.Keyword = searchKeyword ?? ""; // Nếu không có từ khóa, đặt về ""

            var (appointments, totalCount) = await AppointmentService.GetPagedAppointmentsAsync(CurrentPage, PageSize, SearchData.Keyword);

            Appointments = Mapper.Map<List<AppointmentViewModel>>(appointments);

            TotalItems = totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        }


        private async Task HandleSearch()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/appointments?page={CurrentPage}&search={SearchData.Keyword}", forceLoad: true);
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