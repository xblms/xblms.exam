using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesServerController
    {
        [HttpPost, Route(RouteAI)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetAIRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }
            var config = await _configRepository.GetAsync();

            config.AiHostUrl = request.AiHostUrl;
            config.AiServe = request.AiServe;
            config.AiRunningModel = request.AiRunningModel;
            await _configRepository.UpdateAsync(config);

            await _authManager.AddAdminLogAsync("修改AI参数配置");
            await _authManager.AddStatLogAsync(StatType.None, "修改AI参数配置");

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
