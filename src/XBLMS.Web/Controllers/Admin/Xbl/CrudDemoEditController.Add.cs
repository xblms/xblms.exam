using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class CrudDemoEditController
    {
        [HttpPost, Route(RouteAdd)]
        public async Task<ActionResult<BoolResult>> Add([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var info = new CrudDemo
            {
                title = request.Info.title,
                CompanyId = admin.CompanyId,
                DepartmentId = admin.DepartmentId,
                CreatorId = admin.Id
            };
            await _crudDemoRepository.InsertAsync(info);

            await _authManager.AddAdminLogAsync("新增测试数据", $"{info.title}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
