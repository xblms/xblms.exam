using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Utils;
using XBLMS.Core.Utils;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using XBLMS.Enums;

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

            await _authManager.AddAdminLogAsync("修改测试数据", $"修改前:{title},修改后:{info.title}");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
