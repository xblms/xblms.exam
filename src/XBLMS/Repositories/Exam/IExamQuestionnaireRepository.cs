using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamQuestionnaireRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<ExamQuestionnaire> GetAsync(int id);
        Task<ExamQuestionnaire> GetAsync(string guId);
        Task<int> InsertAsync(ExamQuestionnaire item);
        Task<bool> UpdateAsync(ExamQuestionnaire item);
        Task<(int total, List<ExamQuestionnaire> list)> GetListAsync(string keyword, int pageIndex, int pageSize);
        Task<bool> DeleteAsync(int Id);
        Task<int> MaxIdAsync();
        Task IncrementAsync(int id);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount();
    }
}
