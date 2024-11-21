using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class CrudDemoController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var info = await _crudDemoRepository.GetAsync(request.Id);

            await _crudDemoRepository.DeleteAsync(info.Id);

            await _authManager.AddAdminLogAsync("删除测试数据", $"{info.title}");
            

            return new BoolResult
            {
                Value =true
            };
        }
    }
}
