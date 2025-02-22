using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IKnowlegesRepository : IRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<Knowledges> GetAsync(int id);
        Task<int> InsertAsync(Knowledges item);
        Task<bool> UpdateAsync(Knowledges item);
        Task<List<Knowledges>> GetNewListAsync();
        Task<(int total, List<Knowledges> list)> GetListAsync(int userId, int treeId, bool isTreeWithChild, bool like, bool collect, string keyword, int pageIndex, int pageSize);
        Task<(int total, List<Knowledges> list)> GetListAsync(int treeId, bool isTreeWithChild, string keyword, int pageIndex, int pageSize);
        Task<bool> DeleteAsync(int Id);
        Task<int> CountAsync(int treeId, bool withChild = true);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount();
    }
}
