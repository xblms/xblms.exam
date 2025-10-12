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
        public async Task<OrganDepartment> GetDepartmentAsync(int id)
        {
            return await _departmentRepository.GetAsync(id);
        }
        public async Task<List<int>> GetDepartmentIdsAsync(int id)
        {
            return await _departmentRepository.GetIdsAsync(id);
        }
        public async Task<List<string>> GetDepartmentParentPathAsync(int departmentId)
        {
            return await _departmentRepository.GetParentPathAsync(departmentId);
        }
    }
}
