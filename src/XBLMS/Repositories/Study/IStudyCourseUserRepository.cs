using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyCourseUserRepository : IRepository
    {
        Task<int> InsertAsync(StudyCourseUser item);
        Task<bool> UpdateAsync(StudyCourseUser item);
        Task<bool> UpdateByCourseAsync(StudyCourse courseInfo);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByCourseAsync(int courseId);
        Task DeleteByUserId(int userId);
        Task<StudyCourseUser> GetAsync(int id);
        Task<bool> ExistsAsync(int userId, int planId, int courseId);
        Task<StudyCourseUser> GetAsync(int userId, int planId, int courseId);
        Task<(int total, List<StudyCourseUser> list)> GetOfflineListByEventAsync(int userId);
        Task<(int total, List<StudyCourseUser> list)> GetLastListAsync(int userId, string state, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<StudyCourseUser> list)> GetListAsync(int planId, int courseId, string keyWords, string state, int pageIndex, int pageSize);
        Task<(int total, List<StudyCourseUser> list)> GetListAsync(int userId, bool collection, string keyWords, string mark, string orderby, string state, int pageIndex, int pageSize);
        Task<(int total, List<string>)> GetMarkListAsync(int userId);
        Task<int> GetAvgEvaluationAsync(int courseId, int minStar);
        Task<List<StudyCourseUser>> GetListAsync(int planId, int userId);
        Task<(int total, int overTotal)> GetTotalAsync(int userId);
        Task<int> GetTotalDurationAsync(int userId);
        Task<int> GetTaskCountAsync(int userId);
        Task<int> GetOverCountAsync(int planId, bool isSelect);
        Task<(int totalUser, int overTotalUser)> GetOverCountByAnalysisAsync(int planId, int courseId);
        Task<int> GetOverCountByAnalysisAsync(int planId, int courseId, bool? isOver);
        Task<int> GetOverCountAsync(int planId, int userId, bool isSelect);
        Task<decimal> GetOverTotalCreditAsync(int planId, bool isSelect);
        Task<decimal> GetOverTotalCreditAsync(int planId, int userId, bool isSelect);
        Task<(int starUser, int starTotal)> GetEvaluation(int planId, int courseId);

        Task<decimal> Analysis_GetTotalCreditAsync(int userId, string dateFrom, string dateTo);
        Task<(int total, int overTotal)> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo);
        Task<(int total, List<StudyCourseUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);

    }
}
