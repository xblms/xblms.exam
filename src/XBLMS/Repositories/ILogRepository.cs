using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface ILogRepository : IRepository
    {
        Task AddAdminLogAsync(Administrator admin, string ipAddress, string action, string summary = "");

        Task AddUserLogAsync(User user, string ipAddress, string action, string summary = "");

        Task DeleteIfThresholdAsync();

        Task DeleteAllAdminLogsAsync();

        Task DeleteAllUserLogsAsync();


        Task<int> GetUserLogsCountAsync(int userId, string keyword, string dateFrom, string dateTo);

        Task<List<Log>> GetUserLogsAsync(int userId, string keyword, string dateFrom, string dateTo, int offset, int limit);

        Task<int> GetAdminLogsCountAsync(List<int> adminIds, string keyword, string dateFrom, string dateTo);

        Task<List<Log>> GetAdminLogsAsync(List<int> adminIds, string keyword, string dateFrom, string dateTo, int offset, int limit);
    }
}
