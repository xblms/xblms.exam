using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerProfileController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            var userId = request.UserId;
            var adminId = _authManager.AdminId;
            var adminAuth = await _authManager.GetAdminAuth();

            Administrator administrator;
            var last = new Administrator();
            if (userId > 0)
            {
                last = administrator = await _administratorRepository.GetByUserIdAsync(userId);
                if (administrator == null) return this.Error(Constants.ErrorNotFound);
            }
            else
            {
                administrator = new Administrator();
            }

            if (administrator.Id == 0)
            {
                if (!string.IsNullOrEmpty(request.Mobile))
                {
                    var exists = await _administratorRepository.IsMobileExistsAsync(request.Mobile);
                    if (exists)
                    {
                        return this.Error("此手机号码已注册，请更换手机号码");
                    }
                }

                administrator.UserName = request.UserName;
            }
            else
            {
                if (!StringUtils.EqualsIgnoreCase(administrator.Mobile, request.Mobile))
                {
                    if (!string.IsNullOrEmpty(request.Mobile))
                    {
                        var exists = await _administratorRepository.IsMobileExistsAsync(request.Mobile);
                        if (exists)
                        {
                            return this.Error("此手机号码已注册，请更换手机号码");
                        }
                    }
                }
            }

            administrator.DisplayName = request.DisplayName;
            administrator.AvatarUrl = request.AvatarUrl;
            administrator.Mobile = request.Mobile;
            administrator.Email = request.Email;
            administrator.Auth = request.Auth;
            if (administrator.Auth != AuthorityType.AdminNormal)
            {
                administrator.AuthData = AuthorityDataType.DataAll;
            }
            else
            {
                administrator.AuthData = request.AuthData.Value;
            }

            if (request.OrganType == "company")
            {
                administrator.CompanyId = request.OrganId;
                administrator.DepartmentId = 0;
                administrator.DepartmentParentPath = null;

                var company = await _organManager.GetCompanyAsync(request.OrganId);
                if (company != null)
                {
                    administrator.CompanyParentPath = company.CompanyParentPath;
                }
            }
            else
            {
                var department = await _organManager.GetDepartmentAsync(request.OrganId);
                if (department != null)
                {
                    administrator.CompanyId = department.CompanyId;
                    administrator.DepartmentId = department.Id;
                    administrator.DepartmentParentPath = department.DepartmentParentPath;
                }
            }

            administrator.AuthDataCurrentOrganId = administrator.CompanyId;

            if (administrator.Id == 0)
            {
                administrator.CreatorId = adminId;

                var (isValid, errorMessage) = await _administratorRepository.InsertAsync(administrator, request.Password);
                if (!isValid)
                {
                    return this.Error($"管理员添加失败：{errorMessage}");
                }

                administrator = await _administratorRepository.GetByUserNameAsync(request.UserName);
                if (!string.IsNullOrEmpty(administrator.AvatarUrl))
                {
                    var fileName = PageUtils.GetFileNameFromUrl(administrator.AvatarUrl);
                    var filePath = _pathManager.GetAdministratorUploadPath(administrator.UserName, fileName);
                    var avatarFilePath = _pathManager.GetAdministratorUploadPath(administrator.UserName, fileName);
                    FileUtils.CopyFile(filePath, avatarFilePath);
                    administrator.AvatarUrl = _pathManager.GetAdministratorUploadUrl(administrator.UserName, fileName);
                    await _administratorRepository.UpdateAsync(administrator);
                }

                await _authManager.AddAdminLogAsync("新增管理员账号", $"{administrator.DisplayName}");
                await _authManager.AddStatLogAsync(StatType.AdminAdd, "新增管理员账号", administrator.Id, administrator.UserName);
                await _authManager.AddStatCount(StatType.AdminAdd);
            }
            else
            {
                var (isValid, errorMessage) = await _administratorRepository.UpdateAsync(administrator);
                if (!isValid)
                {
                    return this.Error($"管理员修改失败：{errorMessage}");
                }
                await _authManager.AddAdminLogAsync("修改管理员账号", $"{administrator.DisplayName}");
                await _authManager.AddStatLogAsync(StatType.AdminUpdate, "修改管理员账号", administrator.Id, administrator.UserName, last);
                await _authManager.AddStatCount(StatType.AdminUpdate);
            }

            await _administratorsInRolesRepository.DeleteUserAsync(administrator.Id);
            if (administrator.Auth == AuthorityType.AdminNormal)
            {
                if (request.RolesIds != null && request.RolesIds.Count > 0)
                {
                    foreach (var roleId in request.RolesIds)
                    {
                        await _administratorsInRolesRepository.InsertAsync(new AdministratorsInRoles
                        {
                            RoleId = roleId,
                            AdminId = administrator.Id,
                            CompanyId = administrator.CompanyId,
                            DepartmentId = administrator.DepartmentId,
                            CreatorId = adminId,
                        });
                    }
                }
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
