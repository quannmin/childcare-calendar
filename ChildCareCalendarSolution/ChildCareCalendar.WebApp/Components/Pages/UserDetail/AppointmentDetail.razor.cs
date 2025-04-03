using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
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

        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int userIdFromSession;

        [Inject]
        private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var userIdResult = await SessionStorage.GetAsync<int>("userId");
                if (userIdResult.Success)
                {
                    userIdFromSession = userIdResult.Value;

                    Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(userIdFromSession)))?.FirstOrDefault();
                    if (Parent != null && Parent.Role.Equals("PhuHuynh"))
                    {
                        IsAuthenticated = true;
                        await LoadAppointmentsAsync();
                        await LoadExaminationReportAsync();
                    }
                    else
                    {
                        Navigation.NavigateTo("/Login", forceLoad: true);
                    }
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine("Không lấy được dữ liệu userId từ session.");
                    Navigation.NavigateTo("/Login", forceLoad: true);
                }
            }
        }

        private async Task LoadAppointmentsAsync()
        {

            var appointment = await AppointmentService.GetAppointmentByIdAsync(id,
                a => a.Parent,
                a => a.ChildrenRecord,
                a => a.Doctor,
                a => a.FollowUpAppointment,
                a => a.Service
                );

            //Chỉ appointment của parent nào thì parent đó mới được xem
            if (appointment.Parent.Id == id)
            {
                Navigation.NavigateTo("/Login", forceLoad: true);
            }
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