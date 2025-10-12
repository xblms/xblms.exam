using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IPointLogRepository : IRepository
    {
        Task<(int total, List<PointLog> list)> GetListAsync(string dateFrom, string dateTo, string keyWords, int userId, int pageIndex, int pageSize);
        Task InsertAsync(PointLog log);
        Task<int> GetSumPoint(PointType type, int userId, string dateStr);
        Task<(bool success, int point, string notice)> GetNotice(PointType type, int userId);
        Task<(bool success, int point, string notice)> GetNotice(int userId);
        Task<(int total, List<PointLog> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<int> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo);
    }
}
