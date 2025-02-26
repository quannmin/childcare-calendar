using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

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
        public async Task CreateExaminationReportAsync(ExaminationReport examinationReport)
        {
            examinationReport.CreatedAt = DateTime.UtcNow;
            examinationReport.UpdatedAt = DateTime.UtcNow;
            var prescriptionDetails = await _prescriptionDetailRepository
                            .GetAllAsync(pd => pd.ExaminationReportId == examinationReport.Id);

            examinationReport.TotalAmount = prescriptionDetails.Sum(pd => pd.TotalAmount);

            await _examinationRepository.AddAsync(examinationReport);
        }

        public Task<IEnumerable<ExaminationReport>> GetAllExaminationReportsAsync()
        {
            return _examinationRepository.GetAllAsync();
        }

        public async Task<ExaminationReport> GetExaminationReportByIdAsync(int id)
        {
            return await _examinationRepository.GetByIdAsync(id);
        }

    }
}
