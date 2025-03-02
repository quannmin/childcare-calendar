using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IExaminationReportService
    {
        Task<IEnumerable<ExaminationReport>> GetAllExaminationReportsAsync();
        //Task<ExaminationReport> GetExaminationReportByIdAsync(int id);
        Task CreateExaminationReportAsync(ExaminationReport examinationReport);
        Task<IEnumerable<ExaminationReport>> FindExaminationReportsAsync(Expression<Func<ExaminationReport, bool>> predicate, params Expression<Func<ExaminationReport, object>>[] includeProperties);
        Task DeleteExaminationReportAsync(int id);
        Task<ExaminationReport> GetExaminationReportByIdAsync(int id, params Expression<Func<ExaminationReport, object>>[] includes);
    }
}
