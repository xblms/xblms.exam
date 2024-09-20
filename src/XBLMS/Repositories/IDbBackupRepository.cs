using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IDbBackupRepository : IRepository
    {
        Task<(int total, List<DbBackup> list)> GetListAsync(int pageIndex, int pageSize);
        Task<DbBackup> GetAsync(int id);
        Task<int> InsertAsync(DbBackup info);
        Task<bool> UpdateAsync(DbBackup info);
        Task DeleteAsync(int id);
        Task<bool> ExcutionAsync();
    }
}
