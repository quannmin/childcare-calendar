using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IPrescriptionDetailService
    {
        Task<IEnumerable<PrescriptionDetail>> GetAllPrescriptionDetailAsync();
        Task<PrescriptionDetail> GetPrescriptionDetailByIdAsync(int id);
        Task CreatePrescriptionDetailAsync(PrescriptionDetail prescriptionDetail);
        Task<IEnumerable<PrescriptionDetail>> FindPrescriptionDetailsAsync(
              Expression<Func<PrescriptionDetail, bool>> predicate,
              params Expression<Func<PrescriptionDetail, object>>[] includes);
    }
}
