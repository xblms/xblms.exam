using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface ITableStyleRepository : IRepository
    {
        Task<int> InsertAsync(List<int> relatedIdentities, TableStyle style);

        Task UpdateAsync(TableStyle style);

        Task DeleteAllAsync(string tableName);

        Task DeleteAllAsync(string tableName, List<int> relatedIdentities);

        Task DeleteAsync(string tableName, int relatedIdentity, string attributeName);
    }
}
