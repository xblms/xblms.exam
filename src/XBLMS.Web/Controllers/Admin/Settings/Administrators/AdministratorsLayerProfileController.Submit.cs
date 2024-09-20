using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using XBLMS.Configuration;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerProfileController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {

            var userId = request.UserId;

            var adminId = _authManager.AdminId;

            Administrator administrator;
            if (userId > 0)
            {
                administrator = await _administratorRepository.GetByUserIdAsync(userId);
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


            var company = await _organManager.GetCompanyByGuidAsync(request.OrganId);
            var department = await _organManager.GetDepartmentByGuidAsync(request.OrganId);
            var duty = await _organManager.GetDutyByGuidAsync(request.OrganId);
            if (company != null)
            {
                administrator.CompanyId = company.Id;
                administrator.DepartmentId = 0;
                administrator.DutyId = 0;
            }
            if (department != null)
            {
                administrator.CompanyId = department.CompanyId;
                administrator.DepartmentId = department.Id;
                administrator.DutyId = 0;
            }
            if (duty != null)
            {
                administrator.CompanyId = duty.CompanyId;
                administrator.DepartmentId = duty.DepartmentId;
                administrator.DutyId = duty.Id;
            }

            if (administrator.Id == 0)
            {
                administrator.CreatorId = adminId;
                var (isValid, errorMessage) = await _administratorRepository.InsertAsync(administrator, request.Password);
                if (!isValid)
                {
                    return this.Error($"管理员添加失败：{errorMessage}");
                }

                if (!string.IsNullOrEmpty(administrator.AvatarUrl))
                {
                    var fileName = PageUtils.GetFileNameFromUrl(administrator.AvatarUrl);
                    var filePath = _pathManager.GetAdministratorUploadPath(administrator.UserName, fileName);
                    var avatarFilePath = _pathManager.GetAdministratorUploadPath(administrator.UserName, fileName);
                    FileUtils.CopyFile(filePath, avatarFilePath);
                    administrator.AvatarUrl = _pathManager.GetAdministratorUploadUrl(administrator.UserName, fileName);
                    await _administratorRepository.UpdateAsync(administrator);
                }

                await _authManager.AddAdminLogAsync("添加管理员", $"管理员:{administrator.DisplayName}");
            }
            else
            {
                var (isValid, errorMessage) = await _administratorRepository.UpdateAsync(administrator);
                if (!isValid)
                {
                    return this.Error($"管理员修改失败：{errorMessage}");
                }
                await _authManager.AddAdminLogAsync("修改管理员属性", $"管理员:{administrator.DisplayName}");
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
