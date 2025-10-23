using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface ILogRepository : IRepository
    {
        Task AddAdminLogAsync(Administrator admin, string ipAddress, string action, string summary = "");

        Task AddUserLogAsync(User user, string ipAddress, string action, string summary = "");

        Task DeleteIfThresholdAsync();

        Task DeleteAllAdminLogsAsync(AdminAuth auth);

        Task DeleteAllUserLogsAsync(AdminAuth auth);

        Task<(int total, List<Log> list)> GetUserLogsAsync(AdminAuth auth, int userId, string keyword, string dateFrom, string dateTo, int pageIndex, int pageSize);

        Task<(int total, List<Log> list)> GetAdminLogsAsync(AdminAuth auth, List<int> adminIds, string keyword, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<(int total, List<Log> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<int> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo);
    }
}
