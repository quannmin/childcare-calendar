using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IRefundReportService
    {
        Task<IEnumerable<RefundReport>> GetAllRefundReportsAsync();
        Task<RefundReport> GetRefundReportAsync(int id);
        Task AddRefundReportAsync(RefundReport newRefundReport);
        Task<IEnumerable<RefundReport>> FindAppointmentsAsync(Expression<Func<RefundReport, bool>> predicate,
                                                                params Expression<Func<RefundReport, object>>[] includes);
    }
}
