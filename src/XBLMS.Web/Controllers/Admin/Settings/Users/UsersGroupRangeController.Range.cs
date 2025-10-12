using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersGroupRangeController
    {
        [HttpPost, Route(RouteRange)]
        public async Task<ActionResult<BoolResult>> Range([FromBody] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();
            var group = await _userGroupRepository.GetAsync(request.GroupId);

            if (request.Range == 0)//安排
            {
                if(request.RangeUserIds!=null && request.RangeUserIds.Count > 0)
                {
                    foreach (var userId in request.RangeUserIds)
                    {
                        var user = await _userRepository.GetByUserIdAsync(userId);
                        if (user.UserGroupIds != null)
                        {
                            if (!user.UserGroupIds.Contains($"'{group.Id}'"))
                            {
                                user.UserGroupIds.Add($"'{group.Id}'");
                            }
                        }
                        else
                        {
                            user.UserGroupIds = [$"'{group.Id}'"];
                        }
                        await _userRepository.UpdateUserGroupIdsAsync(user);
                    }
                }
                await _authManager.AddAdminLogAsync("用户组安排用户", $"{group.GroupName}");
            }
            else//移出
            {
                if (request.RangeUserIds != null && request.RangeUserIds.Count > 0)
                {
                    foreach (var userId in request.RangeUserIds)
                    {
                        var user = await _userRepository.GetByUserIdAsync(userId);
                        if (user.UserGroupIds != null)
                        {
                            if (user.UserGroupIds.Contains($"'{group.Id}'"))
                            {
                                user.UserGroupIds.Remove($"'{group.Id}'");
                                await _userRepository.UpdateUserGroupIdsAsync(user);
                            }
                        }
                    }
                }
                await _authManager.AddAdminLogAsync("用户组移出用户", $"{group.GroupName}");
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
