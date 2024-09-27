using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Core.Services
{
    public partial class OrganManager
    {
        public async Task<Administrator> GetAdministrator(int adminId)
        {
            var admin = await _administratorRepository.GetByUserIdAsync(adminId);

            var roleNames = await _administratorRepository.GetRoleNames(admin.Id);
            var organNames = await GetOrganName(admin.DutyId, admin.DepartmentId, admin.CompanyId);
            admin.Set("RoleNames", roleNames);
            admin.Set("OrganNames", organNames);
            admin.Set("AuthName", GetAdminAuthName(admin));

            return admin;
        }
        public async Task<User> GetUser(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);

            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);
            user.Set("OrganNames", organNames);

            return user;
        }
        public async Task GetUser(User user)
        {
            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);
            user.Set("OrganNames", organNames);
        }
        private static string GetAdminAuthName(Administrator admin)
        {
            return admin.Auth.GetDisplayName(); 
        }
        public async Task<string> GetUserKeyWords(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);

            return $"{user.UserName}-{user.DisplayName}-{organNames}";
        }
        public async Task<string> GetUserKeyWords(User user)
        {
            var organNames = await GetOrganName(user.DutyId, user.DepartmentId, user.CompanyId);

            return $"{user.UserName}-{user.DisplayName}-{organNames}";
        }
    }
}
