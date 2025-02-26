using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IWorkHour
    {
        Task<IEnumerable<WorkHour>> FindWorkHoursAsync(Expression<Func<WorkHour, bool>> predicate,
                                                                        params Expression<Func<WorkHour, object>>[] includes);
        Task AddWorkHourAsync(WorkHourService workHour);
        Task UpdateWorkHourAsync(WorkHourService workHour);
        Task<int> DeleteWorkHourAsync(int id);
    }
}
