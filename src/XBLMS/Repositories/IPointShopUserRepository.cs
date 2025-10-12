using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IPointShopUserRepository : IRepository
    {
        Task<PointShopUser> GetAsync(int id);
        Task<(int total, List<PointShopUser> list)> GetListAsync(AdminAuth auth, int id, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<PointShopUser> list)> GetListAsync(int userId, string dateFrom, string dateTo, string keyWords, int pageIndex, int pageSize);
        Task<int> InsertAsync(PointShopUser item);
        Task UpdateAsync(PointShopUser item);
        Task<bool> DeleteAsync(int id);
        Task<(int total, List<PointShopUser> list)> Analysis_GetListAsync(int userId, string dateFrom, string dateTo, int pageIndex, int pageSize);
        Task<int> Analysis_GetListAsync(int userId, string dateFrom, string dateTo);
    }
}
