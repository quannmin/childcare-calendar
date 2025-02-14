using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class RefundReportService : IRefundReportService
    {
        private readonly IRepository<RefundReport> _refundReportRepository;

        public RefundReportService(IRepository<RefundReport> refundReportRepository)
        {
            _refundReportRepository = refundReportRepository;
        }

        public async Task AddRefundReportAsync(RefundReport newRefundReport)
        {
            await _refundReportRepository.AddAsync(newRefundReport);
        }

        public async Task<IEnumerable<RefundReport>> GetAllRefundReportsAsync()
        {
            return await _refundReportRepository.GetAllAsync();
        }

        public async Task<RefundReport> GetRefundReportAsync(int id)
        {
            return await _refundReportRepository.GetByIdAsync(id);
        }
    }
}
