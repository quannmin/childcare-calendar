using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services
{
	public class SpecialityService : ISpecialityService
    {
        private readonly IRepository<Speciality> _specialityRepository;

        public SpecialityService(IRepository<Speciality> specialityRepository)
        {
            _specialityRepository = specialityRepository;
        }

        public async Task AddSpecialityAsync(Speciality speciality)
        {
            await _specialityRepository.AddAsync(speciality);
        }

        public async Task DeleteSpecialityAsync(int id)
        {
            var speciality = await _specialityRepository.GetByIdAsync(id);
            if (speciality != null)
            {
                await _specialityRepository.DeleteAsync(speciality);
            }
        }

        public async Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync()
        {
            return await _specialityRepository.GetAllAsync();
        }

        public async Task<Speciality> GetSpecialityByIdAsync(int id)
        {
            return await _specialityRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Speciality>> FindSpeciallityAsync(string findString)
        {
            return await _specialityRepository.FindAsync(x => x.SpecialtyName.ToLower().Contains(findString.ToLower()) || x.Description.ToLower().Contains(findString.ToLower()));
        }

        public async Task UpdateSpecialityAsync(Speciality speciality)
        {
            await _specialityRepository.UpdateAsync(speciality);
        }

		public async Task<bool> CheckDuplicateSpecialtyNameAsync(string name)
		{
            var checkSpeciality = await _specialityRepository.FindAsync(x => x.SpecialtyName.ToLower().Equals(name.ToLower()));
			return checkSpeciality.Count() > 0;
		}

		public async Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync(params Expression<Func<Speciality, object>>[] includes)
		{
			return await _specialityRepository.GetAllAsync(includes);
		}

		public async Task<Speciality> GetSpecialityByIdAsync(int id, params Expression<Func<Speciality, object>>[] includes)
		{
			return await _specialityRepository.GetByIdAsync(id, includes);
		}

        public async Task<bool> CheckDuplicateSpecialtyNameWithNotIdAsync(int id, string name)
        {
            var checkSpeciality = await _specialityRepository.FindAsync(x => x.SpecialtyName.ToLower().Equals(name.ToLower()) && x.Id != id);
            return checkSpeciality.Count() > 0;
        }
    }
}
