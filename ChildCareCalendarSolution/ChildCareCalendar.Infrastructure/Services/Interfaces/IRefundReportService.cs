using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IRefundReportService
    {
        Task<IEnumerable<RefundReport>> GetAllRefundReportsAsync();
        Task<RefundReport> GetRefundReportAsync(int id);
        Task AddRefundReportAsync(RefundReport newRefundReport);
    }
}
