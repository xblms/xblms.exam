using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRepository : IRepository
    {
        Task<List<ExamPaper>> GetAllAsync();
        Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId);
        Task<(int total, int count)> CountAsync(int treeId);
        Task<ExamPaper> GetAsync(int id);
        Task<int> InsertAsync(ExamPaper item);
        Task<bool> UpdateAsync(ExamPaper item);
        Task<List<ExamPaper>> GetListAsync(AdminAuth auth, string keyword);
        Task<(int total, List<ExamPaper> list)> GetListByDateAsync(AdminAuth auth, string dateType, bool onlyTotal = true);
        Task<(int total, List<ExamPaper> list)> GetListAsync(AdminAuth auth, bool treeIsChild, int treeId, string keyword, int pageIndex, int pageSize);
        Task<bool> DeleteAsync(int Id);
        Task<int> MaxAsync();
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCountMoni(AdminAuth auth);
        Task<int> GetGroupCount(int groupId);
        Task<int> GetCerCount(int cerId);
        Task<int> GetTmGroupCount(int groupId);
    }
}
