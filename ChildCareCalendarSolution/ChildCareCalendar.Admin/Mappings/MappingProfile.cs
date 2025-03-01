using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Medicine;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
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
            CreateMap<ChildrenRecordCreateViewModel,  ChildrenRecord>().ReverseMap();
            CreateMap<ServiceCreateViewModel, Service>();
            CreateMap<ServiceEditViewModel, Service>().ReverseMap();
            CreateMap<Service, ServiceViewModel>();

            CreateMap<MedicineViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineEditViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineCreateViewModel, Medicine>();

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
