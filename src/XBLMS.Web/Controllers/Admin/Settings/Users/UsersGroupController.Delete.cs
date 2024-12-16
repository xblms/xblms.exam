using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersGroupController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }
            var group = await _userGroupRepository.GetAsync(request.Id);

            await _userGroupRepository.DeleteAsync(group.Id);

            await _authManager.AddAdminLogAsync("删除用户组", $"{group.GroupName}");
            await _authManager.AddStatLogAsync(StatType.UserGroupDelete, "删除用户组", group.Id, group.GroupName, group);
            await _authManager.AddStatCount(StatType.UserGroupDelete);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
