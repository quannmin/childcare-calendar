using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Domain.ViewModels.Refundreport;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChildCareCalendar.Admin.Components.Pages.RefundReport
{
    public partial class Index
    {
        private List<RefundReportViewModel> RefundReports = new();
        private List<RefundReportViewModel> FilteredReports = new();
        private List<RefundReportViewModel> PagedRefundReports = new();

        private string SearchTerm = "";
        private int CurrentPage = 1;
        private const int PageSize = 10; // Cố định số lượng bản ghi mỗi trang là 10
        private int TotalPages => (int)System.Math.Ceiling((double)FilteredReports.Count / PageSize);

        [Inject] private IRefundReportService RefundReportService { get; set; }
        [Inject] private IMapper Mapper { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadRefundReportsAsync();
        }

        private async Task LoadRefundReportsAsync()
        {
            var reports = await RefundReportService.FindRefundReportsAsync(
                r => !r.IsDelete,
                r => r.Appointment,
                r => r.Appointment.Parent
            );

            RefundReports = Mapper.Map<List<RefundReportViewModel>>(reports);
            FilterReports();
        }

        private void FilterReports()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                FilteredReports = RefundReports
                    .Where(r => r.UserName.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                FilteredReports = new List<RefundReportViewModel>(RefundReports);
            }

            CurrentPage = 1;
            UpdatePagedData();
        }

        private void UpdatePagedData()
        {
            PagedRefundReports = FilteredReports
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            StateHasChanged();
        }

        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                UpdatePagedData();
            }
        }

        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdatePagedData();
            }
        }
    }
}
