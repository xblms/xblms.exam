using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using XBLMS.Models;
using System.Text.RegularExpressions;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmGroupRangeController
    {
        [HttpPost, Route(RouteRange)]
        public async Task<ActionResult<BoolResult>> range([FromBody] GetRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }


            var admin = await _authManager.GetAdminAsync();
            var group = await _userGroupRepository.GetAsync(request.GroupId);
            var userIds = new List<int>();
            if (request.RangeAll)
            {
                var companyIds = new List<int>();
                var departmentIds = new List<int>();
                var dutyIds = new List<int>();
                if (request.OrganId == 0) { }
                else if (request.OrganId == 1 && request.OrganType == "company") { }
                else
                {
                    if (request.OrganId != 0)
                    {
                        if (request.OrganType == "company")
                        {
                            companyIds = await _organManager.GetCompanyIdsAsync(request.OrganId);
                        }
                        if (request.OrganType == "department")
                        {
                            departmentIds = await _organManager.GetDepartmentIdsAsync(request.OrganId);
                        }
                        if (request.OrganType == "duty")
                        {
                            dutyIds = await _organManager.GetDutyIdsAsync(request.OrganId);
                        }
                    }
                }
                var users = await _userRepository.GetUsersAsync(companyIds, departmentIds, dutyIds, group.UserIds, request.Range, request.LastActivityDate, request.Keyword, request.Order, request.Offset, int.MaxValue);
                if (users != null && users.Count > 0)
                {
                    userIds = users.Select(u => u.Id).ToList();

                }

            }
            else
            {
                userIds = request.RangeUserIds;
            }

            if (group.UserIds == null) { group.UserIds = new List<int>(); }
            if (request.Range == 0)//安排
            {
                group.UserIds.AddRange(userIds);
                group.UserIds = group.UserIds.Distinct().ToList();
                await _userGroupRepository.UpdateAsync(group);
                await _authManager.AddAdminLogAsync("安排用户", $"用户组:{group.GroupName}");
            }
            else//移出
            {
                group.UserIds = group.UserIds.Where(id => !userIds.Contains(id)).ToList();
                await _userGroupRepository.UpdateAsync(group);
                await _authManager.AddAdminLogAsync("移出用户", $"用户组:{group.GroupName}");
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
