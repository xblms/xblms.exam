using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersGroupRangeController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResults>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var adminAuth = await _authManager.GetAdminAuth();

            var group = await _userGroupRepository.GetAsync(request.GroupId);

            var (count, users) = await _userRepository.UserGroupRnageGetUserListAsync(adminAuth, request.OrganId, request.OrganType, group.Id, request.Range, request.LastActivityDate, request.Keyword, request.Order, request.PageIndex, request.PageSize);

            if (count > 0)
            {
                foreach (var user in users)
                {
                    await _organManager.GetUser(user);
                }
            }
            return new GetResults
            {
                Users = users,
                Count = count,
                GroupName = group.GroupName
            };
        }
    }
}
