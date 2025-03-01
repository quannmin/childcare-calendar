using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services
{
	public class WorkHourService : IWorkHourService
	{
		private readonly IRepository<WorkHour> _workHourRepository;

		public WorkHourService(IRepository<WorkHour> workHourRepository)
		{
			_workHourRepository = workHourRepository;
		}

		public async Task AddWorkHourAsync(WorkHour workHour)
		{
			await _workHourRepository.AddAsync(workHour);
		}

		public async Task<int> DeleteWorkHourAsync(int id)
		{
			var workHour = await _workHourRepository.GetByIdAsync(id);
			if (workHour != null)
			{
				workHour.IsDelete = true;
				await _workHourRepository.UpdateAsync(workHour, workHour.Id);
				id = workHour.Id;
			}
			return id > 0 ? id : 0;
		}

		public async Task<IEnumerable<WorkHour>> FindWorkHoursAsync(
			Expression<Func<WorkHour, bool>> predicate,
			params Expression<Func<WorkHour, object>>[] includes)
		{
			return await _workHourRepository.FindAsync(predicate, includes);
		}

		public async Task UpdateWorkHourAsync(WorkHour workHour)
		{
			await _workHourRepository.UpdateAsync(workHour, workHour.Id);
		}
	}
}
