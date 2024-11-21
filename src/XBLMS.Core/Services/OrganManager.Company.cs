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
        public async Task<OrganCompany> GetCompanyByGuidAsync(string guid)
        {
            return await _companyRepository.GetByGuidAsync(guid);
        }
        public async Task<List<int>> GetCompanyIdsAsync(int id)
        {
            return await _companyRepository.GetIdsAsync(id);
        }
        public async Task<List<string>> GetCompanyGuidsAsync(List<int> ids)
        {
            if(ids == null || ids.Count == 0) return null;
            return await _companyRepository.GetGuidsAsync(ids);
        }

        public async Task<List<OrganCompany>> GetCompanyListAsync()
        {
            return await _companyRepository.GetListAsync();
        }
    }
}
