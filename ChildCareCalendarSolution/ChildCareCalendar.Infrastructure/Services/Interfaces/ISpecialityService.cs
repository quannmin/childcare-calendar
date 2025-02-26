using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
	public interface ISpecialityService
	{
		Task<IEnumerable<Speciality>> FindSpecialitiesAsync(
			Expression<Func<Speciality, bool>> predicate,
			params Expression<Func<Speciality, object>>[] includes);
		Task AddSpecialityAsync(Speciality speciality);
		Task UpdateSpecialityAsync(Speciality speciality);
		Task DeleteSpecialityAsync(int id);

	}
}
