using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IErrorLogRepository : IRepository
    {
        Task<int> InsertAsync(ErrorLog logInfo);

        Task DeleteIfThresholdAsync();

        Task DeleteAllAsync();

        Task<ErrorLog> GetErrorLogAsync(int logId);

        Task<int> GetCountAsync(string keyword, string dateFrom, string dateTo);

        Task<List<ErrorLog>> GetAllAsync(string keyword, string dateFrom,
            string dateTo, int pageIndex, int pageSie);
    }
}
