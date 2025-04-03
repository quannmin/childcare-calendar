using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Web;

namespace ChildCareCalendar.Admin.Components.Pages.ExaminationReport
{
    public partial class Index
    {
        private List<ExaminationReportViewModel> ExaminationReports = new();

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        IExaminationReportService ExaminationReportService { get; set; }

        [SupplyParameterFromForm]
        private ExaminationReportSearchViewModel SearchData { get; set; } = new();
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

            SearchData.Keyword = queryParameters["search"] ?? "";
            await LoadExaminationReportsAsync();
        }

        private async Task LoadExaminationReportsAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParameters = HttpUtility.ParseQueryString(uri.Query);

     
            var searchKeyword = queryParameters["search"];
            SearchData.Keyword = searchKeyword ?? "";

            var (reports, totalCount) = await ExaminationReportService.GetPagedExaminationReportsAsync(
                CurrentPage,
                PageSize,
                SearchData.Keyword);

            ExaminationReports = Mapper.Map<List<ExaminationReportViewModel>>(reports);

            TotalItems = totalCount;
            TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
        }

        private async Task HandleSearch()
        {
            CurrentPage = 1;
            Navigation.NavigateTo($"/examination-reports?page={CurrentPage}&search={SearchData.Keyword}", forceLoad: true);
            await LoadExaminationReportsAsync();
        }

        private async Task ChangePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages && newPage != CurrentPage)
            {
                CurrentPage = newPage;
                await LoadExaminationReportsAsync();
                Navigation.NavigateTo($"/examination-reports?page={CurrentPage}", forceLoad: false);
            }
        }

        private async void ConfirmDelete(int id)
        {
            idToDelete = id;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        private async Task DeleteExaminationReport()
        {
            if (idToDelete.HasValue)
            {
                await ExaminationReportService.DeleteExaminationReportAsync(idToDelete.Value);
                idToDelete = null;
                await LoadExaminationReportsAsync();
            }
            await JS.InvokeVoidAsync("hideDeleteModal");
        }
    }
}