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
				await _specialityRepository.UpdateAsync(speciality, speciality.SpecialityId);
			}
		}

		public async Task<IEnumerable<Speciality>> FindSpecialitiesAsync(
			Expression<Func<Speciality, bool>> predicate, 
			params Expression<Func<Speciality, object>>[] includes)
		{
			return await _specialityRepository.FindAsync(predicate, includes);
		}

		public async Task UpdateSpecialityAsync(Speciality speciality)
		{
			await _specialityRepository.UpdateAsync(speciality, speciality.SpecialityId);
		}

	}
}
