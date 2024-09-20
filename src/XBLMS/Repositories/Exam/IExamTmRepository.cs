using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;


namespace XBLMS.Repositories
{
    public interface IExamTmRepository: IRepository
    {
        Task<bool> ExistsAsync(string title, int txId);
        Task<int> InsertAsync(ExamTm item);
        Task<bool> UpdateAsync(ExamTm item);
        Task<bool> DeleteAsync(int id);
        Task<List<ExamTm>> GetListAsync(List<int> tmIds);
        Task<List<ExamTm>> GetListWithOutLockedAsync(List<int> tmIds);
        Task<List<ExamTm>> GetListWithOutLockedAsync(List<int> tmIds, int txId, int nandu1Count = 0, int nandu2Count = 0, int nandu3Count = 0, int nandu4Count = 0, int nandu5Count = 0);
        Task<(int total, List<ExamTm> list)> GetListAsync(List<int> withoutIds, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize);
        Task<(int total, List<ExamTm> list)> GetListAsync(ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize);
        Task<int> GetCountAsync(ExamTmGroup group, List<int> treeIds, int txId, int nandu, string keyword, string order, string orderType, bool? isStop, int pageIndex, int pageSize);
        Task<ExamTm> GetAsync(int id);
        Task<int> GetCountByTxIdAsync(int txId);
        Task<int> GetCountByTreeIdAsync(int treeId);
        Task<int> GetCountByTreeIdsAsync(List<int> treeIds);

        Task<int> GetCountByWithoutStopAsync();
        Task<int> GetCountByWithoutStopAndInIdsAsync(List<int> ids);
        Task<int> GetCountAsync(List<int> treeIds,List<int> txIds,List<int> nandus,List<string> zhishidianKeywords,DateTime? dateFrom,DateTime? dateTo);
        Task<List<int>> GetIdsAsync(List<int> treeIds, List<int> txIds, List<int> nandus, List<string> zhishidianKeywords, DateTime? dateFrom, DateTime? dateTo);

        Task<List<int>> GetIdsWithOutLockedAsync();

        Task<int> GetCountAsync(List<int> tmIds, int txId,int nandu);
        Task<List<ExamTm>> GetListByRandomAsync(List<int> tmIds, bool hasTmGroup, int txId, int nandu1Count = 0, int nandu2Count = 0, int nandu3Count = 0, int nandu4Count = 0, int nandu5Count = 0);
    }
}
