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
    public class StatRepository : IStatRepository
    {
        private readonly Repository<Stat> _repository;

        public StatRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<Stat>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task AddCountAsync(StatType statType)
        {
            await AddCountAsync(statType, 0);
        }


        public async Task AddCountAsync(StatType statType, int adminId)
        {
            var lowerDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var higherDate = lowerDate.AddDays(1);

            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate);

            if (adminId > 0)
            {
                query.Where(nameof(Stat.AdminId), adminId);
            }

            if (await _repository.ExistsAsync(query))
            {
                await _repository.IncrementAsync(nameof(Stat.Count), query);
            }
            else
            {
                await _repository.InsertAsync(new Stat
                {
                    StatType = statType,
                    AdminId = adminId,
                    Count = 1
                });
            }
        }

        public async Task<List<Stat>> GetStatsAsync(DateTime lowerDate, DateTime higherDate, StatType statType)
        {
            return await GetStatsAsync(lowerDate, higherDate, statType, 0);
        }

        public async Task<List<Stat>> GetStatsAsync(DateTime lowerDate, DateTime higherDate, StatType statType, int adminId)
        {
            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate.AddDays(1));

            if (adminId > 0)
            {
                query.Where(nameof(Stat.AdminId), adminId);
            }

            return await _repository.GetAllAsync(query);
        }

        public async Task<List<Stat>> GetStatsWithAdminIdAsync(DateTime lowerDate, DateTime higherDate, StatType statType)
        {
            var query = Q
                .Where(nameof(Stat.StatType), statType.GetValue())
                .WhereNot(nameof(Stat.AdminId), 0)
                .WhereBetween(nameof(Stat.CreatedDate), lowerDate, higherDate.AddDays(1));


            return await _repository.GetAllAsync(query);
        }

        public async Task DeleteAllAsync()
        {
            await _repository.DeleteAsync();
        }
    }
}
