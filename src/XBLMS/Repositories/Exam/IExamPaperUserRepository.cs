using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPaperUserRepository : IRepository
    {
        Task<ExamPaperUser> GetAsync(int paperId, int userId);
        Task<ExamPaperUser> GetOnlyOneAsync(int userId);
        Task ClearByUserAsync(int userId);
        Task ClearByPaperAsync(int paperId);
        Task ClearByPaperAndUserAsync(int paperId, int userId);
        Task DeleteAsync(int id);
        Task<int> InsertAsync(ExamPaperUser item);
        Task<bool> ExistsAsync(int paperId, int userId);
        Task<List<int>> GetPaperIdsByUser(int userId, string date);
        Task<List<int>> GetPaperIdsByUser(int userId);
    }
}
