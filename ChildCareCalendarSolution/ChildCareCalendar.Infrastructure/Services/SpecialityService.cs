using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
			if (speciality != null && !speciality.IsDelete)
			{
				speciality.IsDelete = true;
				await _specialityRepository.UpdateAsync(speciality, speciality.Id);
			}
		}

		public async Task<IEnumerable<Speciality>> FindSpecialitiesAsync(
			Expression<Func<Speciality, bool>> predicate, 
			params Expression<Func<Speciality, object>>[] includes)
		{
			return await _specialityRepository.FindAsync(predicate, includes);
		}

        public async Task<(IEnumerable<Speciality> medicines, int totalCount)> GetPagedSpecialitiesAsync(int pageIndex, int pageSize, string keyword = null)
        {
            Expression<Func<Speciality, bool>> filter = null;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.SpecialtyName.Contains(keyword) && !x.IsDelete;
            }
            else
            {
                filter = x => !x.IsDelete;
            }
            return await _specialityRepository.GetPagedAsync(pageIndex, pageSize, filter);
        }

        public async Task UpdateSpecialityAsync(Speciality speciality)
		{
			await _specialityRepository.UpdateAsync(speciality, speciality.Id);
		}

	}
}
