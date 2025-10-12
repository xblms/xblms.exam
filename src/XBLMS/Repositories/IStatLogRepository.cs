using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IStatLogRepository : IRepository
    {
        Task<StatLog> GetAsync(int id);
        Task InsertAsync(Administrator admin, StatType statType, string statTypeStr, string ip, int ohjectId, string objectName, string entity);
        Task<(int total, List<StatLog> list)> GetListAsync(AdminAuth auth, int pageIndex, int pageSize);
    }
}
