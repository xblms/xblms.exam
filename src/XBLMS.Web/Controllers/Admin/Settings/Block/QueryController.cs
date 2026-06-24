using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class QueryController : ControllerBase
    {
        private const string Route = "settings/block/query";

        private readonly IAuthManager _authManager;
        private readonly IBlockManager _blockManager;

        public QueryController(IAuthManager authManager, IBlockManager blockManager)
        {
            _authManager = authManager;
            _blockManager = blockManager;
        }

        public class SubmitRequest
        {
            public string IpAddress { get; set; }
        }

        public class SubmitResult
        {
            public bool IsAllowed { get; set; }
            public BlockArea Area { get; set; }
        }

        

        
    }
}
