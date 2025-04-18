﻿using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Medicine;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Domain.ViewModels.Service;
using ChildCareCalendar.Domain.ViewModels.ServiceVM;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Domain.ViewModels.Refundreport;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using ChildCareCalendar.Domain.ViewModels.WorkHour;
using ChildCareCalendar.Domain.ViewModels.Schedule;

namespace ChildCareCalendar.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SpecialityViewModel, Speciality>().ReverseMap();
            CreateMap<SpecialityCreateViewModel, Speciality>();
            CreateMap<Speciality, SpecialityDetailViewModel>();
            CreateMap<SpecialityEditViewModel, Speciality>().ReverseMap();
            CreateMap<ChildrenRecordCreateViewModel, ChildrenRecord>().ReverseMap();
            CreateMap<ChildrenRecord, ChildrenRecordEditViewModel>();
            CreateMap<ChildrenRecordEditViewModel, ChildrenRecord>().ReverseMap();
            CreateMap<ServiceCreateViewModel, Service>();
            CreateMap<ServiceEditViewModel, Service>().ReverseMap();
            CreateMap<Service, ServiceViewModel>();

            CreateMap<MedicineViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineEditViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineCreateViewModel, Medicine>();

            CreateMap<PrescriptionDetail, PrescriptionDetailViewModel>()
            .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name))
            .ForMember(dest => dest.MedicinePrice, opt => opt.MapFrom(src => src.Medicine.Price))
            .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Slot, opt => opt.MapFrom(src => src.Slot));

            CreateMap<PrescriptionDetailViewModel, PrescriptionDetail>()
            .ForMember(dest => dest.MedicineId, opt => opt.MapFrom(src => src.MedicineId))
            .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Slot, opt => opt.MapFrom(src => src.Slot));

            CreateMap<ExaminationReport, ExaminationReportViewModel>()
            .ForMember(dest => dest.ChildrenName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ChildrenRecord.Gender))
            .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.Appointment.CheckupDateTime))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.PrescriptionDetails, opt => opt.MapFrom(src => src.PrescriptionDetails))
            .ReverseMap();
            CreateMap<ExaminationReport, ExaminationReportDetailViewModel>()
            .ForMember(dest => dest.ChildrenName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ChildrenRecord.Gender))
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.ChildrenRecord.Dob))
            .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.ChildrenRecord.MedicalHistory))
            .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.Appointment.CheckupDateTime))
            .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<ExaminationReportCreateViewModel, ExaminationReport>();

            CreateMap<UserViewModel, AppUser>().ReverseMap();
            CreateMap<UserCreateViewModel, AppUser>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<UserEditViewModel, AppUser>().ReverseMap();
            CreateMap<UserCreateLoginGGViewModel, AppUser>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<AppointmentCreateViewModel, Appointment>();
            CreateMap<Appointment, AppointmentViewModel>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.FullName))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName));

            CreateMap<Appointment, AppointmentDetailViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.FullName))
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ChildrenRecord.Gender))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
            .ForMember(dest => dest.FollowUpAppointment, opt => opt.MapFrom(src => src.FollowUpAppointment.CheckupDateTime));

            CreateMap<Appointment, AppointmentEditViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.CheckupDateTime, opt => opt.MapFrom(src => src.CheckupDateTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<RefundReport, RefundReportViewModel>()
    .ForMember(dest => dest.RefundReportId, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.RefundAmount, opt => opt.MapFrom(src => src.RefundAmount))
    .ForMember(dest => dest.RefundDate, opt => opt.MapFrom(src => src.RefundDate))
    .ForMember(dest => dest.RefundPercentage, opt => opt.MapFrom(src => src.RefundPercentage))
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Appointment.Parent.FullName))
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Appointment.Parent.PhoneNumber));

            CreateMap<WorkHour, WorkHourViewModel>();
            CreateMap<WorkHourCreateViewModel, WorkHour>();
            CreateMap<WorkHourEditViewModel, WorkHour>().ReverseMap();

            CreateMap<Schedule, ScheduleViewModel>().ReverseMap();
            CreateMap<ScheduleCreateViewModel, Schedule>().ReverseMap();
            CreateMap<ScheduleEditViewModel, Schedule>().ReverseMap();
        }
    }
}