using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class ChildrenRecordService : IChildrenRecordService
    {
        private readonly IRepository<ChildrenRecord> _childrenRecordRepository;

        public ChildrenRecordService(IRepository<ChildrenRecord> childrenRecordRepository)
        {
            _childrenRecordRepository = childrenRecordRepository;
        }

        public async Task AddChildrenRecordAsync(ChildrenRecord childrenRecord)
        {
            await _childrenRecordRepository.AddAsync(childrenRecord);
        }

        public async Task<int> DeleteChildrenRecordAsync(int id)
        {
            var childrenRecord = await _childrenRecordRepository.GetByIdAsync(id);
            if (childrenRecord != null)
            {
                childrenRecord.IsDelete = true;
                await _childrenRecordRepository.UpdateAsync(childrenRecord, childrenRecord.Id);
                id = childrenRecord.Id;
            }
            return id > 0 ? id : 0;
        }
        public async Task<IEnumerable<ChildrenRecord>> FindChildrenRecordsAsync(Expression<Func<ChildrenRecord, bool>> predicate,
                                                                      params Expression<Func<ChildrenRecord, object>>[] includes)
        {
            return await _childrenRecordRepository.FindAsync(predicate, includes);
        }

        public async Task UpdateChildrenRecordAsync(int id, ChildrenRecordEditViewModel childrenRecord)
        {
            var exist = await _childrenRecordRepository.GetByIdAsync(id);
            if (exist != null)
            {
                exist.FullName = childrenRecord.FullName;
                exist.Dob = (DateTime)childrenRecord.Dob;
                exist.Gender = childrenRecord.Gender.ToString();
                exist.MedicalHistory = childrenRecord.MedicalHistory;
                await _childrenRecordRepository.UpdateAsync(exist, id);
            }
        }
    }
}
