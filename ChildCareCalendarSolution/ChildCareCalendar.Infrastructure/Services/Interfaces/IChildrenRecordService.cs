using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IChildrenRecordService
    {
        Task<IEnumerable<ChildrenRecord>> FindUsersAsync(Expression<Func<ChildrenRecord, bool>> predicate,
                                                                        params Expression<Func<ChildrenRecord, object>>[] includes);
        Task AddChildrenRecordAsync(ChildrenRecord childrenRecord);
        Task UpdateChildrenRecordAsync(ChildrenRecord childrenRecord);
        Task<int> DeleteChildrenRecordAsync(int id);
    }
}
