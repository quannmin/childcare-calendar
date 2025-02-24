using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Specility;

namespace Pubs.BackendApi.Mappings
{
    public class MappingProfile :  Profile
    {
        public MappingProfile()
        {
            CreateMap<SpecialityViewModel, Speciality>().ReverseMap();
            CreateMap<SpecialityCreateViewModel, Speciality>();
            CreateMap<SpecialityEditViewModel, Speciality>().ReverseMap();

            CreateMap<UserViewModel, AppUser>().ReverseMap();
            CreateMap<UserCreateViewModel, AppUser>();
        }
    }
}
