using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IStudyCourseRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<int> InsertAsync(StudyCourse item);
        Task<int> IncrementTotalUserAsync(int id);
        Task<bool> UpdateAsync(StudyCourse item);
        Task<bool> DeleteAsync(int id);
        Task<StudyCourse> GetAsync(int id);
        Task<StudyCourse> GetByIdAsync(int id);
        Task<(int total, List<StudyCourse> list)> GetListAsync(AdminAuth auth, string keyWords, string type, int treeId, bool children, int pageIndex, int pageSize);
        Task<(int total, List<StudyCourse> list)> GetListByTeacherAsync(int teacherId, string keyWords, int pageIndex, int pageSize);
        Task<int> MaxAsync();
        Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId);
        Task<List<string>> GetMarkListAsync(AdminAuth auth);
        Task<int> GetPaperUseCount(int paperId);
        Task<int> GetPaperQUseCount(int paperId);
        Task<int> GetEvaluationUseCount(int eId);
        Task<(int total, List<StudyCourse> list)> GetOffTrinListByWeekAsync(AdminAuth auth, bool onlyTotal = true);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
    }
}
