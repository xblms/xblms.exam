using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IDbRecoverRepository : IRepository
    {
        Task<(int total, List<DbRecover> list)> GetListAsync(int pageIndex, int pageSize);
        Task<DbRecover> GetAsync(int id);
        Task<int> InsertAsync(DbRecover info);
        Task<bool> UpdateAsync(DbRecover info);
        Task DeleteAsync();
    }
}
