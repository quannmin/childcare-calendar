using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface ISpecialityService
    {
        Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync(params Expression<Func<Speciality, object>>[] includes);
        Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync();
        Task<Speciality> GetSpecialityByIdAsync(int id, params Expression<Func<Speciality, object>>[] includes);
        Task<Speciality> GetSpecialityByIdAsync(int id);
        Task<IEnumerable<Speciality>> FindSpeciallityAsync(string findString);
        Task AddSpecialityAsync(Speciality speciality);
        Task UpdateSpecialityAsync(Speciality speciality);
        Task DeleteSpecialityAsync(int id);
        Task<bool> CheckDuplicateSpecialtyNameAsync(string name);
        Task<bool> CheckDuplicateSpecialtyNameWithNotIdAsync(int id, string name);

    }
}
