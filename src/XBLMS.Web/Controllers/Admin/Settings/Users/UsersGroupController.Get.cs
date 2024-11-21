using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Enums;
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

            var resultGroups = new List<UserGroup>();
            var allGroups = await _userGroupRepository.GetListAsync();

            if (allGroups == null || allGroups.Count == 0)
            {
                await _userGroupRepository.ResetAsync();
                allGroups = await _userGroupRepository.GetListAsync();
            }

            foreach (var group in allGroups)
            {
                var creator = await _administratorRepository.GetByUserIdAsync(group.CreatorId);
                group.Set("TypeName", group.GroupType.GetDisplayName());
                if (creator != null)
                {
                    group.Set("CreatorId", creator.Id);
                    group.Set("CreatorDisplayName", creator.DisplayName);
                }
                if (group.GroupType == UsersGroupType.All)
                {
                    group.UserTotal = await _userRepository.GetCountByUserGroupAsync();
                }
                else if (group.GroupType == UsersGroupType.Fixed)
                {
                    group.UserTotal = await _userRepository.GetCountByUserGroupAsync(group.UserIds);
                }
                else
                {
                    group.UserTotal = await _userRepository.GetCountByUserGroupAsync(group.CompanyIds, group.DepartmentIds, group.DutyIds);
                }

                if (!string.IsNullOrEmpty(request.Search))
                {
                    if (group.GroupName.Contains(request.Search) || StringUtils.Contains(group.Description, request.Search))
                    {
                        resultGroups.Add(group);
                    }
                }
                else
                {
                    resultGroups.Add(group);
                }


            }

            return new GetResult
            {
                Groups = resultGroups
            };
        }
    }
}
