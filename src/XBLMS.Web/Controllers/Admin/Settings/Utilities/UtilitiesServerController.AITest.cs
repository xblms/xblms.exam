using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesServerController
    {
        [HttpGet, Route(RouteAITest)]
        public async Task<ActionResult<GetAIVersionResult>> AITest([FromQuery] GetAIRequest reqeust)
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }
            var config = await _configRepository.GetAsync();
            var (success, versionResult, msg) = await _aiTaskService.ExcutionStatus(reqeust.AiHostUrl);
            var result = new GetAIVersionResult
            {
                Success = success,
                Msg = success ? versionResult.Version : msg
            };
            if (success)
            {
                var (modelsSuccess, modelsResult, modesMsg) = await _aiTaskService.ExcutionRunningModels(reqeust.AiHostUrl);
                if (modelsSuccess)
                {
                    result.IsModels = true;
                    result.Models = modelsResult.Models;
                }
                config.AiHostUrl = reqeust.AiHostUrl;
                config.AiServe = true;
                await _configRepository.UpdateAsync(config);
            }
            return result;
        }
    }
}
