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
            var organNames = await GetOrganName(admin.DepartmentId, admin.CompanyId);
            admin.Set("RoleNames", roleNames);
            admin.Set("OrganNames", organNames);
            admin.Set("AuthName", GetAdminAuthName(admin));

            return admin;
        }
        public async Task<User> GetUser(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);

            var organNames = await GetOrganName(user.DepartmentId, user.CompanyId);
            user.Set("OrganNames", organNames);

            return user;
        }
        public async Task GetUser(User user)
        {
            var organNames = await GetOrganName(user.DepartmentId, user.CompanyId);
            user.Set("OrganNames", organNames);
        }
        private static string GetAdminAuthName(Administrator admin)
        {
            if (admin.AuthData == Enums.AuthorityDataType.DataCreator)
            {
                return $"{admin.Auth.GetDisplayName()}（管理权限内自己创建的资源）";
            }
            return admin.Auth.GetDisplayName();
        }
        public async Task<string> GetUserKeyWords(int userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            var organNames = await GetOrganName(user.DepartmentId, user.CompanyId);

            return $"{user.UserName}-{user.DisplayName}-{user.DutyName}-{organNames}";
        }
    }
}
