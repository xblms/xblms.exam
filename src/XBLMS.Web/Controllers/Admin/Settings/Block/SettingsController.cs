using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class SettingsController : ControllerBase
    {
        private const string Route = "settings/block/settings";
        private const string RouteDelete = "settings/block/settings/actions/delete";

        private readonly IAuthManager _authManager;
        private readonly IBlockRuleRepository _ruleRepository;
        public SettingsController(IAuthManager authManager, IBlockRuleRepository ruleRepository)
        {
            _authManager = authManager;
            _ruleRepository = ruleRepository;
        }

        public class GetResult
        {
            public List<BlockRule> Rules { get; set; }
        }

        public class DeleteRequest
        {
            public int RuleId { get; set; }
        }
    }
}
