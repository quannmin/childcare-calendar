using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Domain.ViewModels.Refundreport;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChildCareCalendar.Admin.Components.Pages.RefundReport
{
    public partial class Index
    {
        private List<RefundReportViewModel> RefundReports = new();

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
            var reports = await RefundReportService.FindAppointmentsAsync(
                r => !r.IsDelete,
                r => r.Appointment,
                r => r.Appointment.Parent
            );

            RefundReports = Mapper.Map<List<RefundReportViewModel>>(reports);
        }
    }
}
