using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamQuestionnaireUserRepository : IRepository
    {
        Task<ExamQuestionnaireUser> GetAsync(int paperId, int userId);
        Task<ExamQuestionnaireUser> GetOnlyOneAsync(int userId);
        Task ClearByUserAsync(int userId);
        Task ClearByPaperAsync(int paperId);
        Task ClearByPaperAndUserAsync(int paperId, int userId);
        Task DeleteAsync(int id);
        Task<int> InsertAsync(ExamQuestionnaireUser item);
        Task<bool> UpdateAsync(ExamQuestionnaireUser item);
        Task<bool> ExistsAsync(int paperId, int userId);
        Task<List<int>> GetPaperIdsByUser(int userId);
        Task<List<int>> GetPaperIdsAsync(int userId);
    }
}
