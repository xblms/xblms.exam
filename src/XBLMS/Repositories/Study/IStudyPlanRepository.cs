using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyPlanRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<int> InsertAsync(StudyPlan item);
        Task<bool> UpdateAsync(StudyPlan item);
        Task<bool> DeleteAsync(int id);
        Task<StudyPlan> GetAsync(int id);
        Task<(int total, List<StudyPlan> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize);
        Task<int> MaxAsync();
        Task<List<int>> GetYearListAsync();
        Task<int> GetPaperUseCount(int paperId);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
        Task<int> GetGroupCount(int groupId);
        Task<(int total, List<StudyPlan> list)> GetListByCreateMAsync(AdminAuth auth, bool onlyTotal = true);
        Task<(int total, List<StudyPlan> list)> GetListByOverMAsync(AdminAuth auth, bool onlyTotal = true);
    }
}
