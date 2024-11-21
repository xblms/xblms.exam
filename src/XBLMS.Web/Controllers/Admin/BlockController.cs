using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class BlockController : ControllerBase
    {
        private const string Route = "blockadmin";

        private readonly ISettingsManager _settingsManager;
        private readonly IBlockManager _blockManager;
        private readonly IBlockRuleRepository _ruleRepository;
        private readonly IConfigRepository _configRepository;

        public BlockController(ISettingsManager settingsManager, IBlockManager blockManager, IBlockRuleRepository ruleRepository, IConfigRepository configRepository)
        {
            _settingsManager = settingsManager;
            _blockManager = blockManager;
            _ruleRepository = ruleRepository;
            _configRepository = configRepository;
        }

        public class QueryRequest
        {
            public string SessionId { get; set; }
        }

        public class QueryResult
        {
            public bool IsAllowed { get; set; }
            public BlockMethod BlockMethod { get; set; }
            public string RedirectUrl { get; set; }
            public string Warning { get; set; }
        }

        public class AuthRequest
        {
            public string Password { get; set; }
        }

        public class AuthResult
        {
            public bool Success { get; set; }
            public string SessionId { get; set; }
        }
    }
}
