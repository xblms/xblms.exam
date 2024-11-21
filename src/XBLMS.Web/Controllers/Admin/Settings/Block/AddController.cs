using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class AddController : ControllerBase
    {
        private const string Route = "settings/block/add";

        private readonly IAuthManager _authManager;
        private readonly IBlockRuleRepository _ruleRepository;
        private readonly IBlockManager _blockManager;

        public AddController(IAuthManager authManager, IBlockRuleRepository ruleRepository, IBlockManager blockManager)
        {
            _authManager = authManager;
            _ruleRepository = ruleRepository;
            _blockManager = blockManager;
        }

        public class GetRequest
        {
            public int RuleId { get; set; }
        }

        public class GetResult
        {
            public BlockRule Rule { get; set; }
            public List<Select<string>> AreaTypes { get; set; }
            public List<IdName> BlockAreas { get; set; }
        }

        public class SubmitRequest
        {
            public BlockRule Rule { get; set; }
            public List<IdName> BlockAreas { get; set; }
        }
    }
}
