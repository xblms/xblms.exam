using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class PointLogRepository
    {
        public async Task<(int total, List<PointLog> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(PointLog.UserId), userId);

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(PointLog.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(PointLog.CreatedDate), "<=", dateTo);
            }

            query.OrderByDesc(nameof(PointLog.Id));

            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));

            return (total, list);
        }

        public async Task<int> Analysis_GetTotalAsync(int userId, string dateFrom, string dateTo)
        {
            var query = Q.Where(nameof(PointLog.UserId), userId);

            if (!string.IsNullOrEmpty(dateFrom))
            {
                query.Where(nameof(PointLog.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                query.Where(nameof(PointLog.CreatedDate), "<=", dateTo);
            }

            query.OrderByDesc(nameof(PointLog.Id));

            var total = await _repository.SumAsync(nameof(PointLog.Point), query);

            return total;
        }
    }
}
