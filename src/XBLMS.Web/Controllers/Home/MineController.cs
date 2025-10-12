using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class MineController : ControllerBase
    {
        private const string Route = "mine";

        private readonly IAuthManager _authManager;
        private readonly ISettingsManager _settingsManager;

        public MineController(IAuthManager authManager, ISettingsManager settingsManager)
        {
            _authManager = authManager;
            _settingsManager = settingsManager;
        }
        public class GetResult
        {
            public User User { get; set; }
            public string Version { get; set; }
        }
    }
}
