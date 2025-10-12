using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganDepartmentRepository
    {
        Task<bool> SyncUpdateAsync(OrganDepartment department);
        Task<int> SyncInsertAsync(OrganDepartment department);
        Task<OrganDepartment> SyncGetBySyncIdAsync(string syncId);
    }
}
