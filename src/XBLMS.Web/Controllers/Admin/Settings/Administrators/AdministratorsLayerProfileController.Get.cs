using Datory;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerProfileController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get([FromQuery] GetRequest request)
        {
            var admin = await _authManager.GetAdminAsync();

            var auths = _authManager.AuthorityTypes();
            var authDatas = _authManager.AuthorityDataTypes();

            var adminAuth = await _authManager.GetAdminAuth();

            if (adminAuth.AuthType != AuthorityType.Admin)
            {
                auths = auths.Where(r => r.Value != AuthorityType.Admin.GetValue()).ToList();
                if (adminAuth.AuthType == AuthorityType.AdminNormal)
                {
                    auths = auths.Where(r => r.Value != AuthorityType.AdminCompany.GetValue()).ToList();
                }
                if (adminAuth.AuthDataType == AuthorityDataType.DataCreator)
                {
                    authDatas = authDatas.Where(r => r.Value != AuthorityDataType.DataAll.GetValue()).ToList();
                }
            }


            var rolesIds = new List<int>();
            var isSelf = false;
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var administrator = await _administratorRepository.GetByUserNameAsync(request.UserName);
                var organId = 0;
                var organName = "";
                var organType = "department";
                if (administrator.DepartmentId > 0)
                {
                    var department = await _organManager.GetDepartmentAsync(administrator.DepartmentId);
                    if (department != null)
                    {
                        organId = department.Id;
                        organName = department.Name;
                        organType = "department";
                    }
                }
                else
                {
                    var company = await _organManager.GetCompanyAsync(administrator.CompanyId);
                    if (company != null)
                    {
                        organId = company.Id;
                        organName = company.Name;
                        organType = "company";
                    }
                }
                if (admin.Id == administrator.Id)
                {
                    isSelf = true;
                }
                rolesIds = await _administratorsInRolesRepository.GetRoleIdsForAdminAsync(administrator.Id);
                return new GetResult
                {
                    UserId = administrator.Id,
                    UserName = administrator.UserName,
                    DisplayName = administrator.DisplayName,
                    AvatarUrl = administrator.AvatarUrl,
                    Mobile = administrator.Mobile,
                    Email = administrator.Email,
                    Auth = administrator.Auth.GetValue(),
                    AuthData = administrator.AuthData.GetValue(),
                    OrganId = organId,
                    OrganName = organName,
                    OrganType = organType,
                    Auths = auths,
                    AuthDatas = authDatas,
                    IsSelf = isSelf,
                    RolesIds = rolesIds,
                    AdminId = admin.Id,
                    CreatorId = administrator.CreatorId
                };
            }

            return new GetResult()
            {
                AuthDatas = authDatas,
                IsSelf = isSelf,
                Auths = auths,
                RolesIds = rolesIds,
            };

        }
    }
}
