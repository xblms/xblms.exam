using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface ICrudDemoRepository : IRepository
    {
        Task<int> InsertAsync(CrudDemo info);

        Task<bool> UpdateAsync(CrudDemo info);
        Task DeleteAsync(int id);
        Task<CrudDemo> GetAsync(int id);

        Task<(int total, List<CrudDemo> list)> ListAsync(string title, int pageIndex, int pageSize);
    }
}
