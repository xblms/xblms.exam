using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganCompanyRepository
    {
        Task<bool> HasChildren(int parentId);
        Task<List<string>> GetParentPathAsync(int id);
        Task<OrganCompany> GetAsync(string name);
        Task<(List<string> path, List<string> names)> GetParentPathAndNamesAsync(int id);
        Task<List<OrganCompany>> GetListAsync(int parentId);
        Task<List<OrganCompany>> GetListAsync(string keywords);
        Task<List<OrganCompany>> GetListAsync(AdminAuth auth, string keywords);
        Task<OrganCompany> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
        Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount(AdminAuth auth);
    }
}
