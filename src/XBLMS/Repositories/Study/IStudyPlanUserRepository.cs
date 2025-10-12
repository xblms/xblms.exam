using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyPlanUserRepository : IRepository
    {
        Task<bool> ExistsAsync(int planId, int userId);
        Task<int> InsertAsync(StudyPlanUser item);
        Task<bool> UpdateAsync(StudyPlanUser item);
        Task<bool> UpdateByPlanAsync(StudyPlan planInfo);
        Task<bool> DeleteAsync(int id);
        Task DeleteByUserId(int userId);
        Task<bool> DeleteByPlanAsync(int planId);
        Task<StudyPlanUser> GetAsync(int id);
        Task<StudyPlanUser> GetAsync(int planId, int userId);
        Task<List<int>> GetUserIdsAsync(int planId);
        Task<(int total, List<StudyPlanUser> list)> GetTaskListAsync(int userId);
        Task<(int total, List<StudyPlanUser> list)> GetListAsync(int year, string state, string keyWords, int userId, int pageIndex, int pageSize);
        Task<(int total, List<StudyPlanUser> list)> GetListAsync(string state, string keyWords, int planId, int pageIndex, int pageSize);
        Task<(decimal totalCredit, decimal totalOverCredit)> GetCreditAsync(int userId);
        Task<(int count, int overCount)> GetCountAsync(int userId);
        Task<int> GetTaskCountAsync(int userId);
        Task<int> GetCountAsync(int planId, string state);
        Task<decimal> GetTotalCreditAsync(int planId);

        Task<(int total, int overTotal, int dabiaoTotal)> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo);
        Task<(int total, List<StudyPlanUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
    }
}
