﻿using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Service;
using ChildCareCalendar.Domain.ViewModels.ServiceVM;
using ChildCareCalendar.Domain.ViewModels.Specility;

namespace Pubs.BackendApi.Mappings
{
    public class MappingProfile :  Profile
    {
        public MappingProfile()
        {
            CreateMap<SpecialityViewModel, Speciality>().ReverseMap();
            CreateMap<SpecialityCreateViewModel, Speciality>();
            CreateMap<Speciality, SpecialityDetailViewModel>();
            CreateMap<SpecialityEditViewModel, Speciality>().ReverseMap();

            CreateMap<ServiceCreateViewModel, Service>();
            CreateMap<ServiceEditViewModel, Service>().ReverseMap();
            CreateMap<Service, ServiceViewModel>();

            CreateMap<UserViewModel, AppUser>().ReverseMap();
            CreateMap<UserCreateViewModel, AppUser>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<UserEditViewModel, AppUser>().ReverseMap();

            CreateMap<AppoinmentCreateViewModel, Appointment>();
            CreateMap<Appointment, AppointmentViewModel>()
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent.FullName))
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.ChildrenRecord.FullName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.ServiceName));
        }
    }
}
