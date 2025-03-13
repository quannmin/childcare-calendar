using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IWorkHourService
    {
        Task<IEnumerable<WorkHour>> FindWorkHoursAsync(Expression<Func<WorkHour, bool>> predicate,
                                                                        params Expression<Func<WorkHour, object>>[] includes);
        Task<IEnumerable<WorkHour>> GetAllWorkHoursAsync();
        Task<WorkHour?> GetWorkHourByIdAsync(int id);
        Task AddWorkHourAsync(WorkHour workHour);
        Task UpdateWorkHourAsync(WorkHour workHour);
        Task<int> DeleteWorkHourAsync(int id);
    }
}
