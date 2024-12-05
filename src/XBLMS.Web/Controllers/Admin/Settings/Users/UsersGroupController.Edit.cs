using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersGroupController
    {
        [HttpGet, Route(RouteEditGet)]
        public async Task<ActionResult<GetEditResult>> GetEdit([FromQuery] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }


            var group = new UserGroup();
            var selectOrganIds = new List<string>();
            if (request.Id > 0)
            {
                group = await _userGroupRepository.GetAsync(request.Id);
                if (group != null && group.GroupType == UsersGroupType.Range)
                {
                    var companyGuids = await _organManager.GetCompanyGuidsAsync(group.CompanyIds);
                    if (companyGuids != null && companyGuids.Count > 0) { selectOrganIds.AddRange(companyGuids); }

                    var departmentGuids = await _organManager.GetDepartmentGuidsAsync(group.DepartmentIds);
                    if (departmentGuids != null && departmentGuids.Count > 0) { selectOrganIds.AddRange(departmentGuids); }

                    var dutyGuids = await _organManager.GetDutyGuidsAsync(group.DutyIds);
                    if (dutyGuids != null && dutyGuids.Count > 0) { selectOrganIds.AddRange(dutyGuids); }
                }
            }
            var organs = await _organManager.GetOrganTreeTableDataAsync();
            var groupTypeSelects = ListUtils.GetSelects<UsersGroupType>();

            return new GetEditResult
            {
                Group = group,
                GroupTypeSelects = groupTypeSelects,
                Organs = organs,
                SelectOrganIds = selectOrganIds
            };
        }


        [HttpPost, Route(RouteEditPost)]
        public async Task<ActionResult<BoolResult>> PostEdit([FromBody] GetEditRequest request)
        {
            if (request.Group.Id > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }


            var admin = await _authManager.GetAdminAsync();

            if (request.Group.GroupType == UsersGroupType.Range)
            {
                var companyIds = new List<int>();
                var departmentIds = new List<int>();
                var dutyIds = new List<int>();
                if (request.SelectOrgans != null && request.SelectOrgans.Count > 0)
                {
                    foreach (var organ in request.SelectOrgans)
                    {
                        if (organ.Type.Equals("company"))
                        {
                            companyIds.Add(organ.Id);
                        }
                        if (organ.Type.Equals("department"))
                        {
                            departmentIds.Add(organ.Id);
                        }
                        if (organ.Type.Equals("duty"))
                        {
                            dutyIds.Add(organ.Id);
                        }
                    }
                }
                request.Group.CompanyIds = companyIds;
                request.Group.DepartmentIds = departmentIds;
                request.Group.DutyIds = dutyIds;
            }
            else
            {
                request.Group.CompanyIds = null;
                request.Group.DepartmentIds = null;
                request.Group.DutyIds = null;
            }

            if (request.Group.Id > 0)
            {
                var group = await _userGroupRepository.GetAsync(request.Group.Id);
                await _userGroupRepository.UpdateAsync(request.Group);
                await _authManager.AddAdminLogAsync("修改用户组", $"{group.GroupName}");

                await _authManager.AddStatLogAsync(StatType.UserGroupUpdate, "修改用户组", group.Id, group.GroupName, group);
            }
            else
            {
                request.Group.CreatorId = admin.Id;
                request.Group.CompanyId = admin.CompanyId;
                request.Group.DepartmentId = admin.DepartmentId;

                var groupId = await _userGroupRepository.InsertAsync(request.Group);
                await _authManager.AddAdminLogAsync("新增用户组", $"{request.Group.GroupName}");
                await _authManager.AddStatLogAsync(StatType.UserGroupAdd, "新增用户组", groupId, request.Group.GroupName);
                await _authManager.AddStatCount(StatType.UserGroupAdd);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
