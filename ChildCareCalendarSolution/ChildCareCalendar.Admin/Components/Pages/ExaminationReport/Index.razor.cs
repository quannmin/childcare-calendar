using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

        protected override async Task OnInitializedAsync()
        {
            await LoadExaminationReportsAsync();
        }

        private async Task LoadExaminationReportsAsync()
        {
            ExaminationReports = Mapper.Map<List<ExaminationReportViewModel>>(await ExaminationReportService.FindExaminationReportsAsync(a => !a.IsDelete,
                            a => a.ChildrenRecord, a => a.Appointment));
        }

        private async Task HandleSearch()
        {
            var reports = string.IsNullOrWhiteSpace(SearchData.Keyword)
                ? await ExaminationReportService.FindExaminationReportsAsync(a => !a.IsDelete, a => a.ChildrenRecord, a => a.Appointment)
                : await ExaminationReportService.FindExaminationReportsAsync(a => SearchData.Keyword.Equals(a.ChildrenRecord.FullName),
                                                                a => a.ChildrenRecord, a => a.Appointment);

            ExaminationReports = Mapper.Map<List<ExaminationReportViewModel>>(reports);
            StateHasChanged();
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