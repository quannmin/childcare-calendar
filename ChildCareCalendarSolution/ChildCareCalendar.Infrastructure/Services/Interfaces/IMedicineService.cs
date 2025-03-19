using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<Medicine>> GetAllMedicinesAsync();
        Task<Medicine> GetMedicineByIdAsync(int id);
        Task CreateMedineAsnync(Medicine medicine);
        Task UpdateMedicineAsync(Medicine medicine);
        Task DeleteMedicineAsync(int id);
        Task<IEnumerable<Medicine>> FindMedicinesAsync(Expression<Func<Medicine, bool>> predicate,
       params Expression<Func<Medicine, object>>[] includes);
        Task<(IEnumerable<Medicine> medicines, int totalCount)> GetPagedMedicinesAsync(
        int pageIndex,
        int pageSize,
        string keyword = null);
    }
}
