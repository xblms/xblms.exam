using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersGroupController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var adminAuth = await _authManager.GetAdminAuth();

            var resultGroups = new List<UserGroup>();
            var allGroups = await _userGroupRepository.GetListAsync(adminAuth, request.Search);
      
            foreach (var group in allGroups)
            {
                var creator = await _administratorRepository.GetByUserIdAsync(group.CreatorId);
                group.Set("TypeName", group.GroupType.GetDisplayName());
                if (creator != null)
                {
                    group.Set("Creator", creator.DisplayName);
                }
                else
                {
                    group.Set("Creator", "/");
                    group.CreatorId = 0;
                }
                group.UserTotal = await _userRepository.UserGroupGetUserCountAsync(group);

                group.Set("UseCount", await _organManager.GetGroupCount(group.Id));

                resultGroups.Add(group);

            }

            return new GetResult
            {
                Groups = resultGroups
            };
        }
    }
}
