using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UtilitiesServerController : ControllerBase
    {
        private const string Route = "settings/serverConfig";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;

        public UtilitiesServerController(ISettingsManager settingsManager, IAuthManager authManager, IConfigRepository configRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _configRepository = configRepository;
        }

        public class GetItem
        {
            public bool BushuFilesServer { get; set; } = false; //课件服务器单独部署
            public string BushuFilesServerUrl { get; set; } = ""; //课件服务器单独部署
            public SystemCode SystemCode { get; set; } = SystemCode.Exam;
            public string SystemCodeName { get; set; }
        }
    }
}
