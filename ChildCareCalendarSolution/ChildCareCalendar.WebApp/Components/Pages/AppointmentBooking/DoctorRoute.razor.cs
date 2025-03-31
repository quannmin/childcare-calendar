using Microsoft.AspNetCore.Components;
using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class DoctorRoute
    {
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int userIdFromSession;

        private int? selectedDoctorId;
        private string? selectedDoctorName;
        private int? selectedSpecialtyId;
        private string? selectedSpecialtyName;
        private int? selectedServiceId;
        private string? selectedServiceName;
        private DateTime? selectedDate;
        private int? selectedWorkHourId;
        private TimeSpan? selectedStartTime;
        private TimeSpan? selectedEndTime;
        private double? selectedServicePrice;
        private string? selectedPaymentMethod;

        private AppUser? selectedDoctor;
        private Speciality? selectedSpeciality;
        private Service? selectedService;

        [Inject] private IUserService UserService { get; set; } = default!;

        [Inject] private IScheduleService ScheduleService { get; set; } = default!;
        [Inject] private IAppointmentService AppointmentService { get; set; } = default!;
        [Inject] private IVnPayService VnPayService { get; set; } = default!;
        [Inject] private IPayPalService PayPalService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

        [Inject]
        private ProtectedSessionStorage SessionStorage { get; set; } = default!;

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
                    }
                    else
                    {
                        Navigation.NavigateTo("/Login", forceLoad: true);
                    }
                    StateHasChanged();
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            if (query.TryGetValue("doctorId", out var doctorIdStr) && int.TryParse(doctorIdStr, out var doctorId))
            {
                selectedDoctor = await UserService.FindUserAsync(
                    u => u.Id == doctorId,
                    u => u.Speciality // Include chuyên khoa
                );

                if (selectedDoctor != null)
                {
                    selectedDoctorId = selectedDoctor.Id;
                    selectedDoctorName = selectedDoctor.FullName;
                    selectedSpecialtyId = selectedDoctor.Speciality?.Id;
                    selectedSpecialtyName = selectedDoctor.Speciality?.SpecialtyName;
                }
            }

            StateHasChanged(); // Đảm bảo UI cập nhật khi doctorId thay đổi
        }


        private void HandleDoctorSelection(AppUser doctor)
        {
            selectedDoctorId = doctor.Id;
            selectedDoctorName = doctor.FullName;
            selectedSpecialtyId = doctor.Speciality.Id;
            selectedSpecialtyName = doctor.Speciality.SpecialtyName;
            selectedDoctor = doctor;
            selectedSpeciality = doctor.Speciality;
            StateHasChanged();
        }

        private void HandleServiceSelection(Service service)
        {
            selectedServiceId = service.Id;
            selectedServiceName = service.ServiceName;
            selectedServicePrice = service.Price;
            selectedService = service;
            StateHasChanged();
        }

        private async Task HandleDateSelection(DateTime date)
        {
            selectedDate = date;
            selectedWorkHourId = null;
            selectedStartTime = null;
            selectedEndTime = null;
            StateHasChanged();
        }

        private async Task HandleSlotSelection(WorkHour workHour)
        {
            selectedWorkHourId = workHour.Id;
            selectedStartTime = workHour.StartTime;
            selectedEndTime = workHour.EndTime;
            StateHasChanged();
        }

        private async Task HandlePaymentSelection(string paymentMethod)
        {
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                selectedPaymentMethod = paymentMethod;
                await ProcessPayment();
            }
        }

        private async Task ProcessPayment()
        {
            if (selectedServicePrice == null || selectedDoctorId == null || selectedDate == null || selectedWorkHourId == null)
                return;

            var schedule = (await ScheduleService.FindSchedulesAsync(
                s => s.UserId == selectedDoctorId &&
                     s.WorkDay.HasValue &&
                     s.WorkDay.Value.Date == selectedDate.Value.Date &&
                     s.WorkHourId == selectedWorkHourId,
                s => s.WorkHour)).FirstOrDefault();

            if (schedule == null)
            {
                Console.WriteLine("❌ Không tìm thấy lịch làm việc tương ứng!");
                return;
            }

            var appointment = new Appointment
            {
                DoctorId = selectedDoctorId.Value,
                ParentId = userIdFromSession,
                ChildrenRecordId = 1,
                ServiceId = selectedServiceId ?? 0,
                TotalAmount = (decimal)selectedServicePrice.Value,
                CheckupDateTime = selectedDate.Value.Date + selectedStartTime!.Value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ScheduleId = schedule.Id,
                Status = "ORDERED"
            };

            await AppointmentService.AddAppointmentAsync(appointment);

            schedule.IsOccupied = true;
            await ScheduleService.UpdateScheduleAsync(schedule);

            string paymentUrl = "";


            if (selectedPaymentMethod == "VNPay")
            {
                paymentUrl = VnPayService.CreatePaymentUrl(appointment, HttpContextAccessor.HttpContext);
            }
            else if (selectedPaymentMethod == "PayPal")
            {
                Console.WriteLine("🔴 Gửi yêu cầu tạo URL PayPal...");

                try
                {
                    paymentUrl = await PayPalService.CreatePaymentUrl(appointment, HttpContextAccessor.HttpContext);
                    Console.WriteLine($"🟢 Nhận được URL từ PayPal: {paymentUrl}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Lỗi khi gọi PayPal API: {ex.Message}");
                }
            }

            if (!string.IsNullOrEmpty(paymentUrl))
            {
                Console.WriteLine($"✅ Điều hướng đến: {paymentUrl}");
                Navigation.NavigateTo(paymentUrl, forceLoad: true);
            }
            else
            {
                Console.WriteLine("❌ Lỗi: Không lấy được URL thanh toán từ PayPal!");
            }
        }

        private void GoBackToDoctorSelection()
        {
            selectedDoctorId = null;
            selectedDoctorName = null;
            selectedSpecialtyId = null;
            selectedSpecialtyName = null;
            selectedDoctor = null;
            selectedSpeciality = null;
        }

        private void GoBackToServiceSelection()
        {
            selectedServiceId = null;
            selectedServiceName = null;
            selectedServicePrice = null;
            selectedService = null;
        }

        private void GoBackToDateSelection()
        {
            selectedDate = null;
            selectedWorkHourId = null;
            selectedStartTime = null;
            selectedEndTime = null;
        }

        private void GoBackToSlotSelection()
        {
            selectedWorkHourId = null;
            selectedPaymentMethod = null;
        }
    }
}
