using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<OrganCompany> GetCompanyAsync(string name)
        {
            return await _companyRepository.GetAsync(name);
        }
        public async Task<OrganCompany> GetCompanyAsync(int id)
        {
            return await _companyRepository.GetAsync(id);
        }
        public async Task<List<int>> GetCompanyIdsAsync(int id)
        {
            return await _companyRepository.GetIdsAsync(id);
        }

        public async Task<List<string>> GetCompanyParentPathAsync(int companyId)
        {
            return await _companyRepository.GetParentPathAsync(companyId);
        }
    }
}
