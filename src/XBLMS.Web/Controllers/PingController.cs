using Microsoft.AspNetCore.Mvc;
using XBLMS.Services;

namespace XBLMS.Web.Controllers
{
    [Route("api/ping")]
    public partial class PingController : ControllerBase
    {
        private const string Route = "";

        private readonly ISettingsManager _settingsManager;

        public PingController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
    }
}
