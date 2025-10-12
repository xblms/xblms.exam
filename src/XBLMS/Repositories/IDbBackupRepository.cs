using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IDbBackupRepository : IRepository
    {
        Task<bool> ExistsBackupingAsync();
        Task<bool> ExistsTodayAsync();
        Task<DbBackup> GetBackupingAsync();
        Task<(int total, List<DbBackup> list)> GetListAsync(int pageIndex, int pageSize);
        Task<DbBackup> GetAsync(int id);
        Task<int> InsertAsync(DbBackup info);
        Task<bool> UpdateAsync(DbBackup info);
        Task DeleteAsync(int id);
    }
}
