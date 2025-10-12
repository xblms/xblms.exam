using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Models;

namespace XBLMS.Services
{
    public interface IOrganManager
    {
        Task<Administrator> GetAdministrator(int adminId);
        Task<User> GetUser(int userId);
        Task GetUser(User user);
        Task<string> GetUserKeyWords(int userId);

        Task<string> GetOrganName(int departmentId, int companyId);
        Task<List<OrganTree>> GetOrganTreeTableDataLazyByChangeAsync(AdminAuth auth, int parentId, string keyWords);
        Task<List<OrganTree>> GetOrganTreeTableDataLazyAsync(AdminAuth auth, int parentId, string organType, bool showAdminTotal = true, bool showUserTotal = true);
        Task<List<OrganTree>> GetOrganTreeTableDataLazySearchAsync(AdminAuth auth, string keyWords, bool showAdminTotal = true, bool showUserTotal = true);
        Task<OrganCompany> GetCompanyAsync(string name);
        Task<OrganCompany> GetCompanyAsync(int id);
        Task<List<string>> GetCompanyParentPathAsync(int companyId);

        Task<OrganDepartment> GetDepartmentAsync(int companyId, string name);
        Task<OrganDepartment> GetDepartmentAsync(int id);
        Task<List<string>> GetDepartmentParentPathAsync(int departmentId);

        Task<int> GetGroupCount(int groupId);
        Task DeleteUser(int userId);
        Task DeleteAdministrator(int adminId);
    }
}
