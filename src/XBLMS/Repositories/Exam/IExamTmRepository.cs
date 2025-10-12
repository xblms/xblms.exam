using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public partial interface IExamTmRepository : IRepository
    {
        Task<List<ExamTm>> GetAllAsync();
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(string title, int txId);
        Task<int> InsertAsync(ExamTm item);
        Task<bool> UpdateAsync(ExamTm item);
        Task UpdateTmGroupIdsAsync(ExamTm item);
        Task UpdateTmGroupIdsAsync(int groupId);
        Task<bool> DeleteAsync(int id);
        Task<(int total, List<ExamTm> list)> GetSelectListAsync(AdminAuth auth, int groupId, bool isSelect, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, int pageIndex, int pageSize);
        Task<(int total, List<ExamTm> list)> GetListAsync(AdminAuth auth, ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize);
        Task<ExamTm> GetAsync(int id);
        Task<int> GetCountByTxIdAsync(int txId);
        Task<(int count, int total)> GetTotalAndCountByTreeIdAsync(AdminAuth auth, int treeId);
        Task<int> GetRealTotalAsync();
        Task<List<int>> GetIdsWithOutLockedAsync(int companyId = 0);

        Task<int> GetCountAsync(bool isAll, List<int> tmIds, int txId, int nandu);
        Task<List<ExamTm>> GetListByRandomAsync(bool allTm, bool hasGroup, List<int> tmIds, int txId, int nandu1Count = 0, int nandu2Count = 0, int nandu3Count = 0, int nandu4Count = 0, int nandu5Count = 0);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
    }
}
