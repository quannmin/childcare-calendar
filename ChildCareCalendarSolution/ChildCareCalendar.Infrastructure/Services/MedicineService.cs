using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IRepository<Medicine> _medicineRepository;

        public MedicineService(IRepository<Medicine> repository)
        {
            _medicineRepository = repository;
        }
        public async Task CreateMedineAsnync(Medicine medicine)
        {
            await _medicineRepository.AddAsync(medicine);
        }

        public async Task DeleteMedicineAsync(int id)
        {
            var medicine = await GetMedicineByIdAsync(id);

            if (medicine != null)
            {
                await _medicineRepository.DeleteAsync(medicine);
            }
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicinesAsync()
        {
            return await _medicineRepository.GetAllAsync();
        }

        public async Task<Medicine> GetMedicineByIdAsync(int id)
        {
            return await _medicineRepository.GetByIdAsync(id);
        }

        public async Task UpdateMedicineAsync(Medicine medicine)
        {
            await _medicineRepository.UpdateAsync(medicine, medicine.Id);
        }
        public async Task<IEnumerable<Medicine>> FindMedicinesAsync(
        Expression<Func<Medicine, bool>> predicate,
        params Expression<Func<Medicine, object>>[] includes)
        {
            return await _medicineRepository.FindAsync(predicate, includes);
        }
        public async Task<(IEnumerable<Medicine> Medicines, int TotalCount)> GetPagedMedicinesAsync(
    int pageIndex,
    int pageSize,
    string keyword = null)
        {
            Expression<Func<Medicine, bool>> filter = null;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.Name.Contains(keyword) && !x.IsDelete;
            }
            else
            {
                filter = x => !x.IsDelete;
            }
            return await _medicineRepository.GetPagedAsync(pageIndex, pageSize, filter);
        }
    }
}
