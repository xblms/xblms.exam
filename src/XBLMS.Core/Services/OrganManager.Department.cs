using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<OrganDepartment> GetDepartmentAsync(int companyId, string name)
        {
            return await _departmentRepository.GetAsync(companyId, name);
        }
        public async Task<List<OrganDepartment>> GetDepartmentListAsync()
        {
            return await _departmentRepository.GetListAsync();
        }
        public async Task<OrganDepartment> GetDepartmentAsync(int id)
        {
            return await _departmentRepository.GetAsync(id);
        }
        public async Task<OrganDepartment> GetDepartmentByGuidAsync(string guid)
        {
            return await _departmentRepository.GetByGuidAsync(guid);
        }
        public async Task<List<int>> GetDepartmentIdsAsync(int id)
        {
            return await _departmentRepository.GetIdsAsync(id);
        }
        public async Task<List<int>> GetDepartmentIdsByCompanyIdAsync(int companyId)
        {
            return await _departmentRepository.GetIdsByCompanyIdAsync(companyId);
        }
        public async Task<List<string>> GetDepartmentGuidsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return null;
            return await _departmentRepository.GetGuidsAsync(ids);
        }
    }
}
