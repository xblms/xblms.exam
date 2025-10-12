using Microsoft.AspNetCore.Mvc;
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
                var delIds = await _companyRepository.GetIdsAsync(organ.Id);
                await _companyRepository.DeleteByIdsAsync(delIds);
                await _organDepartmentRepository.DeleteByCompanyIdsAsync(delIds);
                await _authManager.AddAdminLogAsync("删除单位", organ.Name);

                await _authManager.AddStatLogAsync(StatType.CompanyDelete, "删除单位", organ.Id, organ.Name, organ);
                await _authManager.AddStatCount(StatType.CompanyDelete);
            }

            if (organ.OrganType == "department")
            {
                var delIds = await _organDepartmentRepository.GetIdsAsync(organ.Id);
                await _organDepartmentRepository.DeleteByIdsAsync(delIds);
                await _authManager.AddAdminLogAsync("删除部门", organ.Name);

                await _authManager.AddStatLogAsync(StatType.DepartmentDelete, "删除部门", organ.Id, organ.Name, organ);
                await _authManager.AddStatCount(StatType.DepartmentDelete);
            }


            return new BoolResult
            {
                Value = true,
            };
        }
    }
}
