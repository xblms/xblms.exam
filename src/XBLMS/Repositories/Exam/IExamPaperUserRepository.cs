using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPaperUserRepository : IRepository
    {
        Task<int> CountAsync(int paperId);
        Task<ExamPaperUser> GetAsync(int id);
        Task<ExamPaperUser> GetAsync(int paperId, int userId);
        Task ClearByPaperAsync(int paperId);
        Task DeleteByUserId(int userId);
        Task DeleteAsync(int id);
        Task<int> InsertAsync(ExamPaperUser item);
        Task<bool> ExistsAsync(int paperId, int userId);
        Task<List<int>> GetPaperIdsByUser(int userId);
        Task<(int total, List<ExamPaperUser> list)> GetListAsync(int paperId, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<ExamPaperUser> list)> GetListAsync(int userId, bool isMoni, bool isApp, string date, string keyWords, int pageIndex, int pageSize);

        Task IncrementAsync(int id);
        Task DecrementAsync(int id);
        Task UpdateExamDateTimeAsync(int paperId, DateTime beginDateTime, DateTime endDateTime);
        Task UpdateExamDateTimeByIdAsync(int id, DateTime beginDateTime, DateTime endDateTime);
        Task UpdateExamTimesAsync(int paperId, int examTimes);
        Task UpdateLockedAsync(int paperId, bool locked);
        Task UpdateLockedAppAsync(int paperId, bool locked);
        Task UpdateKeyWordsAsync(int paperId, string keyWords);
        Task UpdateMoniAsync(int paperId, bool moni);
    }
}
