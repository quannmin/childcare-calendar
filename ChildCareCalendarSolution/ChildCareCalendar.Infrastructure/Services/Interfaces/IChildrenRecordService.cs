using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IChildrenRecordService
    {
        Task<IEnumerable<ChildrenRecord>> FindChildrenRecordsAsync(Expression<Func<ChildrenRecord, bool>> predicate,
                                                                        params Expression<Func<ChildrenRecord, object>>[] includes);
        Task AddChildrenRecordAsync(ChildrenRecord childrenRecord);
        Task UpdateChildrenRecordAsync(int id, ChildrenRecordEditViewModel childrenRecord);
        Task<int> DeleteChildrenRecordAsync(int id);
    }
}
