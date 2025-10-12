using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public partial class PointLogRepository : IPointLogRepository
    {
        private readonly Repository<PointLog> _repository;

        public PointLogRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<PointLog>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task InsertAsync(PointLog log)
        {
            await _repository.InsertAsync(log);
        }
        public async Task<int> GetSumPoint(PointType type, int userId, string dateStr)
        {
            return await _repository.SumAsync(nameof(PointLog.Point), Q.
                Where(nameof(PointLog.PointType), type.GetValue()).
                Where(nameof(PointLog.UserId), userId).
                Where(nameof(PointLog.DateStr), dateStr));
        }
        public async Task<(bool success, int point, string notice)> GetNotice(PointType type, int userId)
        {
            var log = await _repository.GetAsync(Q.
                Where(nameof(PointLog.UserId), userId).
                WhereNullOrFalse(nameof(PointLog.IsNotice)).
                Where(nameof(PointLog.PointType), type.GetValue()).OrderByDesc(nameof(PointLog.Id)).ForPage(1, 1));
            if (log != null)
            {
                log.IsNotice = true;
                await _repository.UpdateAsync(log);
                return (true, log.Point, log.Subject);
            }
            return (false, 0, string.Empty);
        }
        public async Task<(bool success, int point, string notice)> GetNotice(int userId)
        {
            var log = await _repository.GetAsync(Q.
                Where(nameof(PointLog.UserId), userId).
                WhereNullOrFalse(nameof(PointLog.IsNotice)).
                OrderByDesc(nameof(PointLog.Id)).ForPage(1, 1));
            if (log != null)
            {
                await _repository.UpdateAsync(Q.
                    Set(nameof(PointLog.IsNotice), true).
                    WhereNullOrFalse(nameof(PointLog.IsNotice)).
                    Where(nameof(PointLog.UserId), userId));

                return (true, log.Point, log.Subject);
            }
            return (false, 0, string.Empty);
        }

        public async Task<(int total, List<PointLog> list)> GetListAsync(string dateFrom, string dateTo, string keyWords, int userId, int pageIndex, int pageSize)
        {
            var query = Q.Where(nameof(PointLog.UserId), userId);

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                query.Where(nameof(PointLog.CreatedDate), ">=", dateFrom);
            }
            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                query.Where(nameof(PointLog.CreatedDate), "<=", dateTo);
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                var like = $"%{keyWords}%";
                query.WhereLike(nameof(PointLog.Subject), like);
            }

            query.OrderByDesc(nameof(PointLog.Id));
            var total = await _repository.CountAsync(query);
            var list=await _repository.GetAllAsync(query.ForPage(pageIndex, pageSize));
            return (total, list);
        }
    }
}
