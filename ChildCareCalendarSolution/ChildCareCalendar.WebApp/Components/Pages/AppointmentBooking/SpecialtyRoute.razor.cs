using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SpecialtyRoute
    {
        private bool isLoading = true;
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int UserId;

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
        private int? selectedDoctorId;

        private Speciality? selectedSpeciality;
        private Service? selectedService;

        [Inject] private IUserService UserService { get; set; } = default!;

        [Inject] private IScheduleService ScheduleService { get; set; } = default!;
        [Inject] private IAppointmentService AppointmentService { get; set; } = default!;
        [Inject] private IVnPayService VnPayService { get; set; } = default!;
        [Inject] private IPayPalService PayPalService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
        [Inject] private IChildrenRecordService ChildrenRecordService { get; set; } = default!;


        [Inject]
        private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var userIdResult = await SessionStorage.GetAsync<int>("userId");
                if (userIdResult.Success)
                {
                    UserId = userIdResult.Value;

                    Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(UserId)))?.FirstOrDefault();
                    if (Parent != null && Parent.Role.Equals("PhuHuynh"))
                    {
                        IsAuthenticated = true;
                        isLoading = false;
                    }
                    else
                    {
                        Navigation.NavigateTo("/Login", forceLoad: true);
                    }
                    StateHasChanged();
                }
                else
                {
                    Navigation.NavigateTo("/Login", forceLoad: true);
                }
                StateHasChanged();
            }
            catch (Exception)
            {
                IsAuthenticated = false;
            }
        }

        private void HandleSpecialtySelection(Speciality speciality)
        {
            selectedSpecialtyId = speciality.Id;
            selectedSpecialtyName = speciality.SpecialtyName;
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

        private async Task HandleDoctorSelection(int doctorId)
        {
            selectedDoctorId = doctorId;
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
            if (selectedServicePrice == null || selectedDate == null || selectedWorkHourId == null || selectedDoctorId == null)
                return;

            var children = await ChildrenRecordService.FindChildrenRecordsAsync(
                    c => c.UserId == UserId && !c.IsDelete
                     );

            var childList = children.ToList();
            if (!childList.Any())
            {
                Console.WriteLine("❌ Không tìm thấy hồ sơ con nào của phụ huynh!");
                return;
            }

            var selectedChild = childList.Count == 1
                ? childList.First()
                : childList[new Random().Next(childList.Count)];

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
                ParentId = UserId,
                ChildrenRecordId = selectedChild.Id,
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
                Navigation.NavigateTo(paymentUrl, forceLoad: true);
            }
            else
            {
                Console.WriteLine("❌ Lỗi: Không lấy được URL thanh toán!");
            }
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
            selectedDoctorId = null;
            selectedPaymentMethod = null;
        }

        void GoBackToSpecialtySelection()
        {
            selectedSpecialtyId = null;
            selectedSpecialtyName = string.Empty;
        }
    }
}