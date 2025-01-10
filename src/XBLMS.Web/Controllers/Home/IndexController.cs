using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class IndexController : ControllerBase
    {
        private const string Route = "index";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IUserMenuRepository _userMenuRepository;

        public IndexController(IAuthManager authManager,
            ISettingsManager settingsManager,
            IConfigRepository configRepository,
            IUserMenuRepository userMenuRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _configRepository = configRepository;
            _userMenuRepository = userMenuRepository;
        }

        public class GetResult
        {
            public User User { get; set; }
            public List<Menu> Menus { get; set; }
            public string Version { get; set; }
        }
    }
}
