using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IMedicineService
    {
        Task<IEnumerable<Medicine>> GetAllMedicinesAsync();
        Task<Medicine> GetMedicineByIdAsync(int id);
        Task CreateMedineAsnync(Medicine medicine);
        Task UpdateMedicineAsync(Medicine medicine);
        Task DeleteMedicineAsync(int id);
    }
}
