using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class LogRepository
    {
        public async Task<(int total, List<Log> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = GetUserQuery(userId, string.Empty, dateFrom, dateTo);

            var total = await _repository.CountAsync(query);

            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }
        public async Task<int> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo)
        {
            var query = GetUserQuery(userId, string.Empty, dateFrom, dateTo);

            var total = await _repository.CountAsync(query);

            return total;

        }
    }
}
