using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class ScheduleService : ISchedule
    {
        private readonly IRepository<Schedule> _repository;
        public ScheduleService(IRepository<Schedule> repository)
        {
            _repository = repository;
        }

        public async Task AddScheduleAsync(Schedule schedule)
        {
            await _repository.AddAsync(schedule);
        }

        public async Task DeleteScheduleAsync(int id)
        {
            var schedule = await _repository.GetByIdAsync(id);
            if (schedule != null && !schedule.IsDelete) {
                schedule.IsDelete = true;
                await _repository.UpdateAsync(schedule, schedule.Id);
            }
        }

        public async Task<IEnumerable<Schedule>> FindSchedulesAsync(Expression<Func<Schedule, bool>> predicate, params Expression<Func<Schedule, object>>[] includes)
        {
            return await _repository.FindAsync(predicate, includes);
        }

        public async Task UpdateScheduleAsync(Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}
