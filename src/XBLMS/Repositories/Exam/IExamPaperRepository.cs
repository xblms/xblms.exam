using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IExamPaperRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<int> GetCountAsync(List<int> treeIds);
        Task<ExamPaper> GetAsync(int id);
        Task<int> InsertAsync(ExamPaper item);
        Task<bool> UpdateAsync(ExamPaper item);
        Task<(int total, List<ExamPaper> list)> GetListAsync(List<int> treeIds, string keyword, int pageIndex, int pageSize);
        Task<bool> DeleteAsync(int Id);
        Task<int> MaxAsync();
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount();
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCountMoni();
    }
}
