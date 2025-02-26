using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

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

           if (medicine !=  null)
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
    }
}
