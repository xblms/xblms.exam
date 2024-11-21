using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Organs
{
    public partial class OrgansController
    {
        [HttpPost, Route(RouteInfoDel)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] GetDeleteRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var organ = request.Organs;
            if (organ.OrganType == "company")
            {
                await _companyRepository.DeleteAsync(organ.Id);
                await _authManager.AddAdminLogAsync("删除单位", organ.Name);
            }

            if (organ.OrganType == "department")
            {
                await _organDepartmentRepository.DeleteAsync(organ.Id);
                await _authManager.AddAdminLogAsync("删除部门", organ.Name);
            }

            await DeleteOrgans(organ.Children);

            return new BoolResult
            {
                Value = true,
            };
        }
        private async Task DeleteOrgans(List<OrganTree> organs)
        {
            if (organs != null && organs.Count > 0)
            {
                foreach (var organ in organs)
                {
                    if (organ.OrganType == "company")
                    {
                        await _companyRepository.DeleteAsync(organ.Id);
                        await _authManager.AddAdminLogAsync("删除单位", organ.Name);
                    }

                    if (organ.OrganType == "department")
                    {
                        await _organDepartmentRepository.DeleteAsync(organ.Id);
                        await _authManager.AddAdminLogAsync("删除部门", organ.Name);
                    }
                    await DeleteOrgans(organ.Children);
                }
            }
        }

    }
}
