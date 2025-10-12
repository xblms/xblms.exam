using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Repositories
{
    public partial class OrganDepartmentRepository
    {
        public async Task<OrganDepartment> SyncGetBySyncIdAsync(string syncId)
        {
            return await _repository.GetAsync(Q.Where(nameof(OrganDepartment.SyncId), syncId));
        }
        public async Task<int> SyncInsertAsync(OrganDepartment department)
        {
            return await InsertAsync(department);
        }
        public async Task<bool> SyncUpdateAsync(OrganDepartment department)
        {
            return await UpdateAsync(department);
        }
    }
}
