using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.ExaminationReport
{
    public partial class Detail
    {
        [Parameter]
        public int ExaminationReportId { get; set; }

        private ExaminationReportDetailViewModel ReportDetail;

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        IExaminationReportService ExaminationReportService { get; set; }

       
        protected override async Task OnParametersSetAsync()
        {
            var report = await ExaminationReportService.GetExaminationReportByIdAsync(ExaminationReportId);
            ReportDetail = Mapper.Map<ExaminationReportDetailViewModel>(report);
            
        }
    }
}