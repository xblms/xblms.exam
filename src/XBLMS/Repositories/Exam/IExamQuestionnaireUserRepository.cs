using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamQuestionnaireUserRepository : IRepository
    {
        Task<ExamQuestionnaireUser> GetAsync(int planId, int courseId, int paperId, int userId);
        Task ClearByPaperAsync(int paperId);
        Task DeleteByUserId(int userId);
        Task<int> InsertAsync(ExamQuestionnaireUser item);
        Task<bool> UpdateAsync(ExamQuestionnaireUser item);
        Task<bool> ExistsAsync(int paperId, int userId);
        Task<(int total, List<ExamQuestionnaireUser> list)> GetListAsync(int userId, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<ExamQuestionnaireUser> list)> GetListAsync(int paperId,string isSubmit, string keyWords, int pageIndex, int pageSize);
        Task UpdateLockedAsync(int paperId, bool locked);
        Task UpdateKeyWordsAsync(int paperId, string keyWords);
        Task UpdateExamDateTimeAsync(int paperId, DateTime beginDateTime, DateTime endDateTime);
        Task<int> CountAsync(int userId);
        Task<(int total, List<ExamQuestionnaireUser> list)> GetTaskAsync(int userId);
        Task<int> GetSubmitUserCountAsync(int planId, int courseId, int paperId);

        Task<(int total, List<ExamQuestionnaireUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<(int total, int submitTotal)> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo);
    }
}
