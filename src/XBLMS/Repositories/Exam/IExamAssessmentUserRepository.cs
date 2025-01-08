using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamAssessmentUserRepository : IRepository
    {
        Task<ExamAssessmentUser> GetAsync(int assId, int userId);
        Task ClearByPaperAsync(int assId);
        Task DeleteByUserId(int userId);
        Task<int> InsertAsync(ExamAssessmentUser item);
        Task<bool> UpdateAsync(ExamAssessmentUser item);
        Task<bool> ExistsAsync(int assId, int userId);
        Task<(int total, List<ExamAssessmentUser> list)> GetListAsync(int userId, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<ExamAssessmentUser> list)> GetListAsync(int assId, string isSubmit, string keyWords, int pageIndex, int pageSize);
        Task UpdateLockedAsync(int assId, bool locked);
        Task UpdateKeyWordsAsync(int assId, string keyWords);
        Task UpdateExamDateTimeAsync(int assId, DateTime beginDateTime, DateTime endDateTime);

        Task<(int total, int submitTotal)> GetCountAsync(int assId);
        Task<(int total, List<ExamAssessmentUser> list)> GetTaskAsync(int userId);
    }
}
