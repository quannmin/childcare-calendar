using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IPrescriptionDetailService
    {
        Task<IEnumerable<PrescriptionDetail>> GetAllPrescriptionDetailAsync();
        Task<PrescriptionDetail> GetPrescriptionDetailByIdAsync(int id);
        Task CreatePrescriptionDetailAsync(PrescriptionDetail prescriptionDetail);
      
    }
}
