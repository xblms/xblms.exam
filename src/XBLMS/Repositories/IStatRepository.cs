using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStatRepository : IRepository
    {
        Task AddUserCountAsync(User user, StatType statType);
        Task AddCountAsync(Administrator admin, StatType statType);

        Task<List<Stat>> GetStatsAsync(AdminAuth auth, DateTime lowerDate, DateTime higherDate, StatType statType);
        Task<List<Stat>> GetStatsAsync(DateTime lowerDate, DateTime higherDate,
            StatType statType);

        Task<int> SumAsync(StatType statType);
        Task<int> SumAsync(StatType statType, AdminAuth auth);
    }
}
