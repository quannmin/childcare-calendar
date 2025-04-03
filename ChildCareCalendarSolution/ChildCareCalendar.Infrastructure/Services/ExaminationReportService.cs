using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.ExaminationReport;
using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class ExaminationReportService : IExaminationReportService
    {
        private readonly IRepository<ExaminationReport> _examinationRepository;
        private readonly IRepository<PrescriptionDetail> _prescriptionDetailRepository;
        public ExaminationReportService(IRepository<ExaminationReport> repository, IRepository<PrescriptionDetail> prescriptionDetailRepository)
        {
            _examinationRepository = repository;
            _prescriptionDetailRepository = prescriptionDetailRepository;
        }
        public async Task CreateExaminationReportAsync(ExaminationReport examinationReport, List<PrescriptionDetail> prescriptionDetails)
        {

            
            examinationReport.CreatedAt = DateTime.UtcNow;
            examinationReport.UpdatedAt = DateTime.UtcNow;
         
            examinationReport.TotalAmount = prescriptionDetails.Sum(pd => pd.TotalAmount);

            await _examinationRepository.AddAsync(examinationReport);
      
            foreach (var pd in prescriptionDetails)
            {
                pd.ExaminationReportId = examinationReport.Id;
                await _prescriptionDetailRepository.AddAsync(pd);
            }
        }

        public Task<IEnumerable<ExaminationReport>> GetAllExaminationReportsAsync()
        {
            return _examinationRepository.GetAllAsync();
        }

        public async Task<ExaminationReport> GetExaminationReportByIdAsync(int id, params Expression<Func<ExaminationReport, object>>[] includes)
        {
            var query = _examinationRepository.GetQueryable(); 

        
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            query = query
            .Include(r => r.ChildrenRecord)
            .Include(r => r.Appointment)
            .Include(r => r.PrescriptionDetails)
            .ThenInclude(pd => pd.Medicine);

            return await query.FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<IEnumerable<ExaminationReport>> FindExaminationReportsAsync(Expression<Func<ExaminationReport, bool>> predicate, params Expression<Func<ExaminationReport, object>>[] includeProperties)
        {
            return await _examinationRepository.FindAsync(predicate, includeProperties);
        }
        public async Task DeleteExaminationReportAsync(int id)
        {
            var report = await GetExaminationReportByIdAsync(id);
            if (report != null)
            {
                report.IsDelete = true; 
                report.UpdatedAt = DateTime.UtcNow;
                await _examinationRepository.UpdateAsync(report, report.Id);
            }
        }
        public async Task<int?> GetFirstExaminationReportIdByAppointmentIdAsync(int appointmentId)
        {
            return await _examinationRepository.GetQueryable()
                .Where(r => r.AppointmentId == appointmentId && !r.IsDelete)
                .Select(r => (int?)r.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ExaminationReport>> GetExaminationReportsByAppointmentIdAsync(int appointmentId)
        {
            var query = _examinationRepository.GetQueryable()
                .Include(r => r.Appointment)
                .Include(r => r.PrescriptionDetails)
                .ThenInclude(pd => pd.Medicine)
                .Where(r => r.AppointmentId == appointmentId && !r.IsDelete);

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<ExaminationReport> reports, int totalCount)> GetPagedExaminationReportsAsync(
    int pageIndex,
    int pageSize,
    string keyword = null)
        {
            Expression<Func<ExaminationReport, bool>> filter = x => !x.IsDelete;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x =>
                    !x.IsDelete &&
                    (x.ChildrenRecord.FullName.Contains(keyword) ||
                     x.Appointment.Parent.FullName.Contains(keyword) ||
                     x.Appointment.ChildrenRecord.FullName.Contains(keyword));
            }

            return await _examinationRepository.GetPagedAsync(
                pageIndex,
                pageSize,
                filter,
                x => x.ChildrenRecord,
                x => x.Appointment,
                x => x.Appointment.Parent,
                x => x.Appointment.Doctor,
                x => x.Appointment.Service
            );
        }

    }
}
