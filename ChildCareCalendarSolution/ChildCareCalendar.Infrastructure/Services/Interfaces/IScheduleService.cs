using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> FindSchedulesAsync(
            Expression<Func<Schedule, bool>> predicate,
            params Expression<Func<Schedule, object>>[] includes);
        Task AddScheduleAsync(Schedule schedule);
        Task UpdateScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(int id);
    }
}
