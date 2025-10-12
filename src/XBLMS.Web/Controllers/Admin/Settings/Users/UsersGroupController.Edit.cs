using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

            var adminAuth = await _authManager.GetAdminAuth();

            var group = new UserGroup()
            {
                GroupType = UsersGroupType.All
            };

            var groupTypeSelects = ListUtils.GetSelects<UsersGroupType>();
            if (adminAuth.AuthType != AuthorityType.Admin)
            {
                group.GroupType = UsersGroupType.Range;
                groupTypeSelects = groupTypeSelects.Where(g => g.Value != UsersGroupType.All.GetValue()).ToList();
            }

            var selectOrganIds = new List<string>();
            var organs = new List<SelectOrgans>();
            if (request.Id > 0)
            {
                group = await _userGroupRepository.GetAsync(request.Id);
                if (group != null && group.GroupType == UsersGroupType.Range)
                {
                    if (group.CompanyIds != null && group.CompanyIds.Count > 0)
                    {
                        foreach (var companyId in group.CompanyIds)
                        {
                            var company = await _organManager.GetCompanyAsync(companyId);
                            organs.Add(new SelectOrgans
                            {
                                Id = companyId,
                                Name = company.Name,
                                Type = "company"
                            });

                        }
                    }
                    if (group.DepartmentIds != null && group.DepartmentIds.Count > 0)
                    {
                        foreach (var departmentId in group.DepartmentIds)
                        {
                            var department = await _organManager.GetDepartmentAsync(departmentId);
                            organs.Add(new SelectOrgans
                            {
                                Id = department.Id,
                                Name = department.Name,
                                Type = "department"
                            });
                        }
                    }
                }
            }

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
            }
            else
            {
                request.Group.CompanyIds = null;
                request.Group.DepartmentIds = null;
            }
            var exists = await _userGroupRepository.ExistsAsync(request.Group.GroupName, admin.CompanyId);
            if (request.Group.Id > 0)
            {
                var group = await _userGroupRepository.GetAsync(request.Group.Id);

                if (exists && !StringUtils.Equals(group.GroupName, request.Group.GroupName))
                {
                    return this.Error("已存在相同名称的用户组");
                }

                await _userGroupRepository.UpdateAsync(request.Group);
                await _authManager.AddAdminLogAsync("修改用户组", $"{group.GroupName}");

                await _authManager.AddStatLogAsync(StatType.UserGroupUpdate, "修改用户组", group.Id, group.GroupName, group);
            }
            else
            {
                if (exists)
                {
                    return this.Error("已存在相同名称的用户组");
                }

                request.Group.CreatorId = admin.Id;
                request.Group.CompanyId = admin.CompanyId;
                request.Group.DepartmentId = admin.DepartmentId;
                request.Group.CompanyParentPath = admin.CompanyParentPath;
                request.Group.DepartmentParentPath = admin.DepartmentParentPath;

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
