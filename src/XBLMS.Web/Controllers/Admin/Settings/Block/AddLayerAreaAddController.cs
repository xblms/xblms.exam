using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
    public partial class AddLayerAreaAddController : ControllerBase
    {
        private const string Route = "settings/block/addLayerAreaAdd";

        private readonly IAuthManager _authManager;
        private readonly IBlockManager _blockManager;

        public AddLayerAreaAddController(IAuthManager authManager, IBlockManager blockManager)
        {
            _authManager = authManager;
            _blockManager = blockManager;
        }

        public class GetResult
        {
            public List<Select<int>> Areas { get; set; }
        }

        public class SubmitRequest
        {
            public List<int> AreaIds { get; set; }
        }

        public class SubmitResult
        {
            public List<IdName> Areas { get; set; }
        }
    }
}
