using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IPointShopRepository : IRepository
    {
        Task<PointShop> GetAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<(int total, List<PointShop> list)> GetListAsync(AdminAuth auth, string keyWords, int pageIndex, int pageSize);
        Task<(int total, List<PointShop> list)> GetListAsync(int companyId, int point, string keyWords, int pageIndex, int pageSize);
        Task<int> InsertAsync(PointShop item);
        Task UpdateAsync(PointShop item);
        Task<bool> DeleteAsync(int id);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
    }
}
