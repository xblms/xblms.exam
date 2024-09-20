using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Block
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class AnalysisController : ControllerBase
    {
        private const string Route = "settings/block/analysis";

        private readonly IAuthManager _authManager;
        private readonly IBlockAnalysisRepository _analysisRepository;

        public AnalysisController(IAuthManager authManager, IBlockAnalysisRepository analysisRepository)
        {
            _authManager = authManager;
            _analysisRepository = analysisRepository;
        }

        public class GetResult
        {
            public List<string> Days { get; set; }
            public List<int> UserCount { get; set; }
            public List<int> AdminCount { get; set; }
        }
    }
}
