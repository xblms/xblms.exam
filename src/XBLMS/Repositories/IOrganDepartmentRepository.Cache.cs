using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganDepartmentRepository
    {
        Task<bool> HasChildren(int parentId, int companyId);
        Task<List<OrganDepartment>> GetListAsync(int parentId, int companyId);
        Task<List<OrganDepartment>> GetListAsync(string keyWords);
        Task<List<OrganDepartment>> GetListAsync(AdminAuth auth, string keywords);
        Task<List<string>> GetParentPathAsync(int id);
        Task<(List<string> path, List<string> names)> GetParentPathAndNamesAsync(int id);
        Task<List<OrganDepartment>> GetListAsync();
        Task<OrganDepartment> GetAsync(int companyId, string name);
        Task<OrganDepartment> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
    }
}
