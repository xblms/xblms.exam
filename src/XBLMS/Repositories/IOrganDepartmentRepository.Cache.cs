using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganDepartmentRepository
    {
        Task<List<OrganDepartment>> GetListAsync();
        Task<OrganDepartment> GetAsync(int companyId, string name);
        Task<List<OrganDepartment>> GetPathNamesAsync(int id);
        Task<List<OrganDepartment>> GetListAsync(int companyId);
        Task<OrganDepartment> GetAsync(int id);
        Task<OrganDepartment> GetByGuidAsync(string guid);
        Task<List<int>> GetIdsAsync(int id);
        Task<List<int>> GetIdsByCompanyIdAsync(int companyId);
        Task<List<string>> GetGuidsAsync(List<int> ids);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount();
    }
}
