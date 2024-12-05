using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IDbRecoverRepository : IRepository
    {
        Task<(int total, List<DbRecover> list)> GetListAsync(int pageIndex, int pageSize);
        Task<int> InsertAsync(DbRecover info);
        Task DeleteAsync();
    }
}
