using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Users
{
    public partial class UsersLayerProfileController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            var userId = request.UserId;

            if (userId > 0)
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
                {
                    return this.NoAuth();
                }
            }
            else
            {
                if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
                {
                    return this.NoAuth();
                }
            }

            var adminId = _authManager.AdminId;

            User user;
            User last = new User();
            if (userId > 0)
            {
                last = user = await _userRepository.GetByUserIdAsync(userId);
                if (user == null) return this.Error(Constants.ErrorNotFound);
            }
            else
            {
                user = new User();
            }

            user.DisplayName = request.DisplayName;
            user.AvatarUrl = request.AvatarUrl;
            user.Mobile = request.Mobile;
            user.Email = request.Email;
            user.Locked = request.Locked;
            user.DutyName = request.DutyName;

            if (request.OrganType == "company")
            {
                var company = await _organManager.GetCompanyAsync(request.OrganId);
                user.CompanyId = company.Id;
                user.DepartmentId = 0;
                user.DepartmentParentPath = null;
                user.CompanyParentPath = company.CompanyParentPath;
            }
            else
            {
                var department = await _organManager.GetDepartmentAsync(request.OrganId);
                user.CompanyId = department.CompanyId;
                user.DepartmentId = department.Id;
                user.DepartmentParentPath = department.DepartmentParentPath;

                var company = await _organManager.GetCompanyAsync(department.CompanyId);
                user.CompanyParentPath = company.CompanyParentPath;
            }

            if (request.UserId == 0)
            {
                user.UserName = request.UserName;
                user.CreatorId = adminId;

                if (!string.IsNullOrEmpty(request.Mobile))
                {
                    var exists = await _userRepository.IsMobileExistsAsync(request.Mobile);
                    if (exists)
                    {
                        return this.Error("此手机号码已注册，请更换手机号码");
                    }
                }

                var (resultUser, errorMessage) = await _userRepository.InsertAsync(user, request.Password, true, string.Empty);
                if (resultUser == null)
                {
                    return this.Error($"用户添加失败：{errorMessage}");
                }

                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    var fileName = PageUtils.GetFileNameFromUrl(user.AvatarUrl);
                    var filePath = _pathManager.GetUserUploadPath(0, fileName);
                    var avatarFilePath = _pathManager.GetUserUploadPath(resultUser.Id, fileName);
                    FileUtils.CopyFile(filePath, avatarFilePath);
                    user.AvatarUrl = _pathManager.GetUserUploadUrl(resultUser.Id, fileName);
                    await _userRepository.UpdateAsync(resultUser);
                }

                await _authManager.AddAdminLogAsync("新增用户账号", $"{request.UserName}");
                await _authManager.AddStatLogAsync(StatType.UserAdd, "新增用户账号", user.Id, user.DisplayName);
                await _authManager.AddStatCount(StatType.UserAdd);
            }
            else
            {
                if (!StringUtils.EqualsIgnoreCase(user.Mobile, request.Mobile))
                {
                    if (!string.IsNullOrEmpty(request.Mobile))
                    {
                        var exists = await _userRepository.IsMobileExistsAsync(request.Mobile);
                        if (exists)
                        {
                            return this.Error("此手机号码已注册，请更换手机号码");
                        }
                    }
                }

                var (success, errorMessage) = await _userRepository.UpdateAsync(user);
                if (!success)
                {
                    return this.Error($"用户修改失败：{errorMessage}");
                }

                await _authManager.AddAdminLogAsync("修改用户账号", $"{request.UserName}");
                await _authManager.AddStatLogAsync(StatType.UserUpdate, "修改用户账号", user.Id, user.DisplayName, last);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
