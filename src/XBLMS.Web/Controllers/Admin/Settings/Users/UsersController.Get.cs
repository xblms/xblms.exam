using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersController
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
            var (total, list) = await _userRepository.GetListAsync(adminAuth, request.OrganId, request.OrganType, group, request.LastActivityDate, request.Keyword, request.Order, request.Offset, request.Limit);

            if (total > 0)
            {
                foreach (var user in list)
                {
                    await _organManager.GetUser(user);
                }
            }
            return new GetResults
            {
                Users = list,
                Count = total,
            };
        }

        [HttpGet, Route(RouteOtherData)]
        public async Task<ActionResult<GetResults>> GetOtherData()
        {
            var adminAuth = await _authManager.GetAdminAuth();
            var groups = await _userGroupRepository.GetListAsync(adminAuth, true);
            return new GetResults
            {
                Groups = groups
            };
        }
    }
}
