﻿using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Medicine;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Domain.ViewModels.Service;
using ChildCareCalendar.Domain.ViewModels.ServiceVM;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;

namespace Pubs.BackendApi.Mappings
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
            CreateMap<ServiceCreateViewModel, Service>();
            CreateMap<ServiceEditViewModel, Service>().ReverseMap();
            CreateMap<Service, ServiceViewModel>();

            CreateMap<MedicineViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineEditViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineCreateViewModel, Medicine>();

            CreateMap<PrescriptionDetail, PrescriptionDetailViewModel>()
            .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.Medicine.Name)) 
            .ForMember(dest => dest.MedicinePrice, opt => opt.MapFrom(src => src.Medicine.Price))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Quantity * src.Medicine.Price))
            .ForMember(dest => dest.Slot, opt => opt.MapFrom(src => src.Slot));


            CreateMap<ExaminationReport, ExaminationReportViewModel>()
            .ForMember(dest => dest.ChildrenName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ChildrenRecord.Gender))
            .ForMember(dest => dest.ExamDate, opt => opt.MapFrom(src => src.Appointment.CheckupDateTime))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
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

            CreateMap<AppointmentCreateViewModel, Appointment>();
            CreateMap<Appointment, AppointmentViewModel>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.FullName))
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName));

            CreateMap<Appointment, AppointmentDetailViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.FullName))
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
            .ForMember(dest => dest.FollowUpAppointment, opt => opt.MapFrom(src => src.FollowUpAppointment.CheckupDateTime));

            CreateMap<Appointment, AppointmentEditViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.CheckupDateTime, opt => opt.MapFrom(src => src.CheckupDateTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
