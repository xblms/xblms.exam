using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamAssessmentRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<ExamAssessment> GetAsync(int id);
        Task<ExamAssessment> GetAsync(string guId);
        Task<int> InsertAsync(ExamAssessment item);
        Task<bool> UpdateAsync(ExamAssessment item);
        Task<(int total, List<ExamAssessment> list)> GetListAsync(string keyword, int pageIndex, int pageSize);
        Task<bool> DeleteAsync(int Id);
        Task<int> MaxIdAsync();
        Task IncrementAsync(int id);
        Task<int> GetConfigCount(int configId);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount();
        Task<int> GetGroupCount(int groupId);
    }
}
