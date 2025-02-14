using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IRefundReportService
    {
        Task<IEnumerable<RefundReport>> GetAllRefundReportsAsync();
        Task<RefundReport> GetRefundReportAsync(int id);
        Task AddRefundReportAsync(RefundReport newRefundReport);
    }
}
