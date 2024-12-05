using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamQuestionnaireUserRepository : IRepository
    {
        Task<ExamQuestionnaireUser> GetAsync(int paperId, int userId);
        Task ClearByPaperAsync(int paperId);
        Task<int> InsertAsync(ExamQuestionnaireUser item);
        Task<bool> UpdateAsync(ExamQuestionnaireUser item);
        Task<bool> ExistsAsync(int paperId, int userId);
        Task<(int total, List<ExamQuestionnaireUser> list)> GetListAsync(int userId, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<ExamQuestionnaireUser> list)> GetListAsync(int paperId,string isSubmit, string keyWords, int pageIndex, int pageSize);
        Task UpdateLockedAsync(int paperId, bool locked);
        Task UpdateKeyWordsAsync(int paperId, string keyWords);
        Task UpdateExamDateTimeAsync(int paperId, DateTime beginDateTime, DateTime endDateTime);
        Task<int> GetTaskCountAsync(int userId);
    }
}
