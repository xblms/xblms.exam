using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesServerController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] GetItem request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Update))
            {
                return this.NoAuth();
            }
            var config = await _configRepository.GetAsync();

            config.BushuFilesServer = request.BushuFilesServer;
            config.BushuFilesServerUrl = request.BushuFilesServerUrl;
            config.SystemCode = request.SystemCode;
            config.SystemCodeName = request.SystemCodeName;

            await _configRepository.UpdateAsync(config);

            await _authManager.AddAdminLogAsync("修改系统参数配置");
            await _authManager.AddStatLogAsync(StatType.None, "修改系统参数配置");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
