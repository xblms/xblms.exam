using Datory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStatLogRepository : IRepository
    {
        Task<StatLog> GetAsync(int id);
        Task InsertAsync(StatType statType, string statTypeStr, string ip, int adminId, int ohjectId, string objectName, string entity);
        Task<(int total, List<StatLog> list)> GetListAsync(DateTime? lowerDate, DateTime? higherDate, int adminId, int pageIndex, int pageSize);
    }
}
