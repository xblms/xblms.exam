using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class CrudDemoEditController
    {
        [HttpPost, Route(RouteUpdate)]
        public async Task<ActionResult<BoolResult>> UpdateRole([FromBody] SubmitRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }
            var info = await _crudDemoRepository.GetAsync(request.Info.Id);
            var title = info.title;
            info.title = request.Info.title;

            await _crudDemoRepository.UpdateAsync(info);

            await _authManager.AddAdminLogAsync("修改测试数据", $"{title}>{info.title}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
