using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStatRepository : IRepository
    {
        Task AddCountAsync(StatType statType);

        Task AddCountAsync(StatType statType, int adminId);

        Task<List<Stat>> GetStatsAsync(DateTime lowerDate, DateTime higherDate,
            StatType statType);

        Task<List<Stat>> GetStatsAsync(DateTime lowerDate, DateTime higherDate,
            StatType statType, int adminId);

        Task DeleteAllAsync();
    }
}
