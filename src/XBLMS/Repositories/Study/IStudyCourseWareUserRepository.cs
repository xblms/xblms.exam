using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStudyCourseWareUserRepository : IRepository
    {
        Task<int> InsertAsync(StudyCourseWareUser item);
        Task<bool> UpdateAsync(StudyCourseWareUser item);
        Task<bool> DeleteAsync(int id);
        Task DeleteByUserId(int userId);
        Task<bool> ExistsAsync(int userId, int planId, int courseId, int wareId);
        Task<bool> ClearCureentAsync(int userId, int planId, int courseId);
        Task<StudyCourseWareUser> GetAsync(int id);
        Task<List<StudyCourseWareUser>> GetListAsync(int userId, int planId, int courseId);
        Task<(int total, List<StudyCourseWareUser> list)> GetListAsync(string keyWords, int pageIndex, int pageSize);

        Task<int> Analysis_GetTotalDurationAsync(int userId, string dateFrom, string dateTo);
        Task<int> Analysis_GetTotalDurationAsync(int userId, int planId, int courseId);
    }
}
