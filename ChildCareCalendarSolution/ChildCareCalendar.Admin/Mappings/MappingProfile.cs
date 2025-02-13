using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels;

namespace Pubs.BackendApi.Mappings
{
    public class MappingProfile :  Profile
    {
        public MappingProfile()
        {
            CreateMap<SpecialityViewModel, Speciality>().ReverseMap();
            CreateMap<SpecialityCreateViewModel, Speciality>();
        }
    }
}
