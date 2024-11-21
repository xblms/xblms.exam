using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganCompanyRepository
    {
        Task<OrganCompany> GetAsync(string name);
        Task<List<OrganCompany>> GetPathNamesAsync(int id);
        Task<List<OrganCompany>> GetListAsync();
        Task<OrganCompany> GetAsync(int id);
        Task<OrganCompany> GetByGuidAsync(string guid);
        Task<List<int>> GetIdsAsync(int id);
        Task<List<string>> GetGuidsAsync(List<int> ids);
    }
}
