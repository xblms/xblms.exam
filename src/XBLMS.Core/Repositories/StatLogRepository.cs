using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class StatLogRepository : IStatLogRepository
    {
        private readonly Repository<StatLog> _repository;

        public StatLogRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<StatLog>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<StatLog> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task InsertAsync(StatType statType, string statTypeStr, string ip, int adminId, int ohjectId, string objectName, string entity)
        {
            await _repository.InsertAsync(new StatLog
            {
                IpAddress = ip,
                StatType = statType,
                StatTypeStr = statTypeStr,
                AdminId = adminId,
                ObjectId = ohjectId,
                ObjectName = objectName,
                LastEntity = entity
            });
        }


        public async Task<(int total, List<StatLog> list)> GetListAsync(DateTime? lowerDate, DateTime? higherDate, int adminId, int pageIndex, int pageSize)
        {
            var query = Q.NewQuery();

            if (adminId > 0)
            {
                query.Where(nameof(Stat.AdminId), adminId);
            }
            if (lowerDate.HasValue)
            {
                query.Where(nameof(Stat.CreatedDate), ">=", lowerDate.Value);
            }

            if (higherDate.HasValue)
            {
                query.Where(nameof(Stat.CreatedDate), "<=", higherDate.Value);
            }
            var total = await _repository.CountAsync(query);
            var list = await _repository.GetAllAsync(query.OrderByDesc(nameof(Stat.Id)).ForPage(pageIndex, pageSize));
            return (total, list);
        }

    }
}
