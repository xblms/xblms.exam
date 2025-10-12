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

            return new GetItem
            {
                BushuFilesServer = config.BushuFilesServer,
                BushuFilesServerUrl = config.BushuFilesServerUrl,
                SystemCode = config.SystemCode,
                SystemCodeName = config.SystemCodeName,
            };
        }
    }
}
