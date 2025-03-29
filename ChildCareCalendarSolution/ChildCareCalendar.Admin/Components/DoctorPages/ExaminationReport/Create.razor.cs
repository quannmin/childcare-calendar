using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Domain.ViewModels.Medicine;
using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.Admin.Components.DoctorPages.ExaminationReport
{
    public partial class Create
    {
        [Parameter]
        public int AppointmentId { get; set; }

        [Inject]
        private IAppointmentService AppointmentService { get; set; }

        [Inject]
        private IExaminationReportService ExaminationReportService { get; set; }

        [Inject]
        private IMedicineService MedicineService { get; set; }

        [Inject]
        private IPrescriptionDetailService PrescriptionDetailService { get; set; }

        [Inject]
        private IMapper Mapper { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        private AppointmentDetailViewModel Appointment { get; set; } = new();
        private ExaminationReportCreateViewModel NewExaminationReport = new();
        private List<MedicineViewModel> Medicines = new();
        private List<PrescriptionDetailViewModel> PrescriptionDetails = new();
        private List<ExaminationReportViewModel> PreviousExaminationReports = new();
        private int? SelectedMedicineId;
        private PrescriptionDetailViewModel CurrentMedicine = new();
        private string SearchTerm { get; set; } = string.Empty;
        private List<MedicineViewModel> FilteredMedicines { get; set; } = new();
        private bool IsLoading = true;
        private string ErrorMessage = string.Empty;
        private string MedicineError { get; set; } = string.Empty;
        private string MedicineValidationError { get; set; } = string.Empty;
        private bool ShowMedicineModal = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Lấy thông tin cuộc hẹn
                var appointmentEntity = await AppointmentService.GetAppointmentByIdAsync(AppointmentId,
                        a => a.Parent,
                        a => a.ChildrenRecord,
                        a => a.Doctor,
                        a => a.FollowUpAppointment
                        );

                Appointment = Mapper.Map<AppointmentDetailViewModel>(appointmentEntity);

                // Lấy danh sách thuốc
                var medicines = await MedicineService.GetAllMedicinesAsync();
                Medicines = Mapper.Map<List<MedicineViewModel>>(medicines);

                // Lấy lịch sử khám bệnh
                var previousReports = await ExaminationReportService.GetExaminationReportsByAppointmentIdAsync(AppointmentId);
                PreviousExaminationReports = Mapper.Map<List<ExaminationReportViewModel>>(previousReports);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi tải dữ liệu: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnSearchInput(ChangeEventArgs e)
        {
            SearchTerm = e.Value?.ToString() ?? string.Empty;
            FilterMedicines();
        }

        private void FilterMedicines()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                FilteredMedicines = Medicines.Take(5).ToList();
            }
            else
            {
                FilteredMedicines = Medicines
                    .Where(m => m.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .Take(10)
                    .ToList();
            }
        }

        private void SelectMedicine(MedicineViewModel medicine)
        {
            SelectedMedicineId = medicine.Id;
            SearchTerm = medicine.Name;
            FilteredMedicines.Clear();
        }

        private void ResetSearchForm()
        {
            SearchTerm = string.Empty;
            FilteredMedicines.Clear();
            SelectedMedicineId = null;
        }

        private void AddMedicine()
        {
            if (SelectedMedicineId.HasValue && CurrentMedicine.Dosage > 0 && CurrentMedicine.Quantity > 0 && !string.IsNullOrEmpty(CurrentMedicine.Slot))
            {
                var medicine = Medicines.FirstOrDefault(m => m.Id == SelectedMedicineId.Value);
                if (medicine != null)
                {
                    PrescriptionDetails.Add(new PrescriptionDetailViewModel
                    {
                        MedicineId = medicine.Id,
                        MedicineName = medicine.Name,
                        Dosage = CurrentMedicine.Dosage,
                        Quantity = CurrentMedicine.Quantity,
                        Slot = CurrentMedicine.Slot,
                        TotalAmount = medicine.Price * CurrentMedicine.Quantity
                    });

                    // Reset form
                    ResetSearchForm();
                    CurrentMedicine = new PrescriptionDetailViewModel();
                    MedicineValidationError = string.Empty;
                }
                else
                {
                    MedicineValidationError = "Thuốc không tồn tại.";
                }
            }
            else
            {
                MedicineValidationError = "Vui lòng điền đầy đủ thông tin thuốc.";
            }
        }

        private void RemoveMedicine(PrescriptionDetailViewModel medicine)
        {
            PrescriptionDetails.Remove(medicine);
        }

        private async Task SaveExaminationReport()
        {
            try
            {
                if (PrescriptionDetails.Any())
                {
                    var examinationReport = Mapper.Map<Domain.Entities.ExaminationReport>(NewExaminationReport);
                    var prescriptionDetails = Mapper.Map<List<PrescriptionDetail>>(PrescriptionDetails);

                    var appointmentEntity = await AppointmentService.GetAppointmentByIdAsync(AppointmentId);
                    if (appointmentEntity == null)
                    {
                        throw new Exception($"Appointment với ID {AppointmentId} không tồn tại.");
                    }            

                    examinationReport.AppointmentId = AppointmentId;
                    examinationReport.ChildrenRecordId = (int)appointmentEntity.ChildrenRecordId;

                    foreach (var pd in prescriptionDetails)
                    {
                        var medicineExists = await MedicineService.GetMedicineByIdAsync(pd.MedicineId);
                        if (medicineExists == null)
                        {
                            throw new Exception($"Medicine với ID {pd.MedicineId} không tồn tại.");
                        }
                    }

                    await ExaminationReportService.CreateExaminationReportAsync(examinationReport, prescriptionDetails);

                    appointmentEntity.Status = AppointmentStatus.Completed.ToString();
                    await AppointmentService.UpdateAppointmentAsync(appointmentEntity);

                    Navigation.NavigateTo($"/examination-reports/detail/{examinationReport.Id}");
                }
                else
                {
                    MedicineError = "Vui lòng thêm ít nhất một loại thuốc vào đơn thuốc.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi lưu kết quả khám: {ex.Message}";
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            }
        }

        private void Cancel()
        {
            Navigation.NavigateTo($"/appointments/detail/{AppointmentId}");
        }

        private void OpenMedicineModal()
        {
            ShowMedicineModal = true;
        }

        private void CloseMedicineModal()
        {
            ShowMedicineModal = false;
            MedicineValidationError = string.Empty;
        }
    }
}