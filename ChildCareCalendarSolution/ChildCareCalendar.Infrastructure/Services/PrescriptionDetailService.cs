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
    public class PrescriptionDetailService : IPrescriptionDetailService
    {
        private readonly IRepository<PrescriptionDetail> _prescriptionDetailRepository;
        private readonly IRepository<Medicine> _medicineRepository;
        public PrescriptionDetailService(IRepository<PrescriptionDetail> repository, IRepository<Medicine> medicineRepository)
        {
            _prescriptionDetailRepository = repository;
            _medicineRepository = medicineRepository;
        }

        public async Task CreatePrescriptionDetailAsync(PrescriptionDetail prescriptionDetail)
        {
            var medicine = await _medicineRepository.GetByIdAsync(prescriptionDetail.MedicineId);
            if (medicine == null)
            {
                throw new Exception("Medicine not found!");
            }

            prescriptionDetail.TotalAmount = medicine.Price*prescriptionDetail.Quantity;


            await _prescriptionDetailRepository.AddAsync(prescriptionDetail);
        }

        public async Task<IEnumerable<PrescriptionDetail>> GetAllPrescriptionDetailAsync()
        {
            return await _prescriptionDetailRepository.GetAllAsync();
        }

        public async Task<PrescriptionDetail> GetPrescriptionDetailByIdAsync(int id)
        {
            return await _prescriptionDetailRepository.GetByIdAsync(id);
        }

    }
}
