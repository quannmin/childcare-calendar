using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IPrescriptionDetailService
    {
        Task<IEnumerable<PrescriptionDetail>> GetAllPrescriptionDetailAsync();
        Task<PrescriptionDetail> GetPrescriptionDetailByIdAsync(int id);
        Task CreatePrescriptionDetailAsync(PrescriptionDetail prescriptionDetail);
      
    }
}
