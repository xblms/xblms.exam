using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamCerUserRepository : IRepository
    {
        Task<bool> ExistsAsync(int userID, int examPaperId);
        Task<bool> ExistsAsync(int userID, int examPaperId, int planId, int courseId);
        Task DeleteByUserId(int userId);
        Task DeleteByPaperId(int examPaperId);
        Task<ExamCerUser> GetAsync(int id);
        Task<int> InsertAsync(ExamCerUser item);
        Task<(int total, List<ExamCerUser> list)> GetListAsync(int userId, int pageIndex, int pageSize);
        Task<int> UpdateImgAsync(int id, string img);
        Task<int> GetCountAsync(int cerId);
        Task<(int total, List<ExamCerUser> list)> GetListAsync(int cerId, string keyWords, string beginDate, string endDate, int pageIndex, int pageSize);
        Task<(int total, List<ExamCerUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<int> CountAsync(int userId);
        Task<int> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo);
    }
}
