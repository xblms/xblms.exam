using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganDepartmentRepository : IRepository
    {
        Task<int> InsertAsync(OrganDepartment department);

        Task<bool> UpdateAsync(OrganDepartment department);
        Task<bool> DeleteAsync(int id);

    }
}
