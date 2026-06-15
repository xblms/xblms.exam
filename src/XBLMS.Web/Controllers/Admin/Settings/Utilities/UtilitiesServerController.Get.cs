using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    public partial class UtilitiesServerController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetItem>> Get()
        {
            if (!await _authManager.HasPermissionsAsync())
            {
                return this.NoAuth();
            }

            var config = await _configRepository.GetAsync();

            var result = new GetItem
            {
                SystemCodeName = config.SystemCodeName,
                SystemCode = config.SystemCode,
                AiHostUrl = config.AiHostUrl,
                AiServe = config.AiServe,
                AiRunningModel = config.AiRunningModel,
                IsModels = !string.IsNullOrEmpty(config.AiRunningModel)
            };

            return result;
        }
    }
}
