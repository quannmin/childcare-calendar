using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
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
        Task CreateExaminationReportAsync(ExaminationReport examinationReport, List<PrescriptionDetail> prescriptionDetails);
        Task<IEnumerable<ExaminationReport>> FindExaminationReportsAsync(Expression<Func<ExaminationReport, bool>> predicate, params Expression<Func<ExaminationReport, object>>[] includeProperties);
        Task DeleteExaminationReportAsync(int id);
        Task<ExaminationReport> GetExaminationReportByIdAsync(int id, params Expression<Func<ExaminationReport, object>>[] includes);
       
        Task<IEnumerable<ExaminationReport>> GetExaminationReportsByAppointmentIdAsync(int appointmentId);
        Task<(IEnumerable<ExaminationReport> reports, int totalCount)> GetPagedExaminationReportsAsync(
                int pageIndex,
                int pageSize,
                string keyword = null);
    }
}
