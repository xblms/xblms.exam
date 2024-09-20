using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Configuration;
using XBLMS.Enums;

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
            if (userId > 0)
            {
                user = await _userRepository.GetByUserIdAsync(userId);
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


            var company = await _organManager.GetCompanyByGuidAsync(request.OrganId);
            var department = await _organManager.GetDepartmentByGuidAsync(request.OrganId);
            var duty = await _organManager.GetDutyByGuidAsync(request.OrganId);
            if (company != null)
            {
                user.CompanyId = company.Id;
                user.DepartmentId = 0;
                user.DutyId = 0;
            }
            if (department != null)
            {
                user.CompanyId = department.CompanyId;
                user.DepartmentId = department.Id;
                user.DutyId = 0;
            }
            if (duty != null)
            {
                user.CompanyId = duty.CompanyId;
                user.DepartmentId = duty.DepartmentId;
                user.DutyId = duty.Id;
            }

            if (request.UserId == 0)
            {
                user.UserName= request.UserName;
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

                await _authManager.AddAdminLogAsync("添加用户", $"用户:{ request.UserName }");
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

                await _authManager.AddAdminLogAsync("修改用户", $"用户:{ request.UserName }");
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
