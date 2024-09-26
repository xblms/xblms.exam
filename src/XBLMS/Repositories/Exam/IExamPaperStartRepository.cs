using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public  interface IExamPaperStartRepository : IRepository
    {
        Task<ExamPaperStart> GetAsync(int id);
        Task<int> CountAsync(int paperId, int userId);
        Task<ExamPaperStart> GetNoSubmitAsync(int paperId, int userId);
        Task<int> GetNoSubmitIdAsync(int paperId, int userId);
        Task<int> InsertAsync(ExamPaperStart item);
        Task ClearByUserAsync(int userId);
        Task ClearByPaperAsync(int paperId);
        Task ClearByPaperAndUserAsync(int paperId, int userId);
        Task DeleteAsync(int id);
        Task UpdateAsync(ExamPaperStart item);
        Task IncrementAsync(int id);
        Task<List<ExamPaperStart>> GetListAsync(int paperId, int userId);
        Task<(int total, List<ExamPaperStart> list)> GetListAsync(int userId, string dateFrom, string dateTo, string keyWords, int pageIndex, int pageSize);
        Task<List<int>> GetPaperIdsAsync(int userId);
        Task<int?> GetMaxScoreAsync(int userId, int paperId);
        Task UpdateLockedAsync(int paperId, bool locked);
        Task UpdateKeyWordsAsync(int paperId, string keyWords);
    }
}
