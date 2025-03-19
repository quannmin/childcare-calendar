using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChildCareCalendar.WebApp.Components.Pages.UserDetail
{
    public partial class AppointmentDetail
    {
        [Parameter]
        public int id { get; set; }

        private AppointmentDetailViewModel AppointmentViewModel { get; set; } = new();

        private List<PrescriptionDetailViewModel> PrescriptionDetails { get; set; } = new();

        private ExaminationReport Report { get; set; } = new();

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private IExaminationReportService ExaminationReportService { get; set; } = default!;

        [Inject]
        private IPrescriptionDetailService PrescriptionDetailService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;


        protected override async Task OnInitializedAsync()
        {
            await LoadAppointmentsAsync();
            await LoadExaminationReportAsync();
        }

        private async Task LoadAppointmentsAsync()
        {

            var appointment = await AppointmentService.GetAppointmentByIdAsync(id,
                a => a.Parent,
                a => a.ChildrenRecord,
                a => a.Doctor,
                a => a.FollowUpAppointment
                );
            AppointmentViewModel = Mapper.Map<AppointmentDetailViewModel>(appointment);

        }

        private async Task LoadExaminationReportAsync()
        {
            Report = (await ExaminationReportService.FindExaminationReportsAsync(e => e.AppointmentId.Equals(id), e => e.ChildrenRecord)).FirstOrDefault();
            if (Report != null)
            {
                List<PrescriptionDetail> list = (await PrescriptionDetailService.FindPrescriptionDetailsAsync(p => p.ExaminationReportId.Equals(Report.Id), p => p.Medicine)).ToList();
                PrescriptionDetails = Mapper.Map<List<PrescriptionDetailViewModel>>(list);
            }
        }
    }
}