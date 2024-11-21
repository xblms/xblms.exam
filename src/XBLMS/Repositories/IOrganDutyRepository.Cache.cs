using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganDutyRepository
    {
        Task<OrganDuty> GetAsync(int companyId, int departmentId, string name);
        Task<List<OrganDuty>> GetPathNamesAsync(int id);
        Task<List<OrganDuty>> GetListAsync(int departmentId);
        Task<OrganDuty> GetAsync(int id);
        Task<OrganDuty> GetByGuidAsync(string guid);
        Task<List<int>> GetIdsAsync(int id);
        Task<List<int>> GetIdsByDepartmentIdAsync(int departmentId);
        Task<List<string>> GetGuidsAsync(List<int> ids);
    }
}
