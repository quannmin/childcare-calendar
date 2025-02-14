using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IExaminationReportService
    {
        Task<IEnumerable<ExaminationReport>> GetAllExaminationReportsAsync();
        Task<ExaminationReport> GetExaminationReportByIdAsync(int id);
        Task CreateExaminationReportAsync(ExaminationReport examinationReport);
    }
}
