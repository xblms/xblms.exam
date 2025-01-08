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

        Task<string> GetOrganName(int dutyId,int departmentId, int companyId);


        Task<List<OrganTree>> GetOrganTreeTableDataAsync();
        Task<List<OrganCompany>> GetCompanyListAsync();
        Task<OrganCompany> GetCompanyAsync(string name);
        Task<OrganCompany> GetCompanyAsync(int id);
        Task<OrganCompany> GetCompanyByGuidAsync(string guid);
        Task<List<int>> GetCompanyIdsAsync(int id);
        Task<List<string>> GetCompanyGuidsAsync(List<int> ids);

        Task<List<OrganDepartment>> GetDepartmentListAsync();
        Task<OrganDepartment> GetDepartmentAsync(int companyId, string name);
        Task<OrganDepartment> GetDepartmentAsync(int id);
        Task<OrganDepartment> GetDepartmentByGuidAsync(string guid);
        Task<List<int>> GetDepartmentIdsAsync(int id);
        Task<List<int>> GetDepartmentIdsByCompanyIdAsync(int companyId);
        Task<List<string>> GetDepartmentGuidsAsync(List<int> ids);



        Task<OrganDuty> GetDutyAsync(int companyId,int departmentId,string name);
        Task<OrganDuty> GetDutyAsync(int id);
        Task<OrganDuty> GetDutyByGuidAsync(string guid);
        Task<List<int>> GetDutyIdsAsync(int id);
        Task<List<int>> GetDutyIdsByDepartmentIdAsync(int departmentId);
        Task<List<string>> GetDutyGuidsAsync(List<int> ids);

        Task<int> GetGroupCount(int groupId);
        Task DeleteUser(int userId);
    }
}
