using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class KnowledgesController : ControllerBase
    {
        private const string Route = "knowledges";
        private const string RouteItem = Route + "/item";
        private const string RouteTree = Route + "/tree";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IKnowlegesRepository _knowlegesRepository;
        private readonly IKnowlegesTreeRepository _knowlegesTreeRepository;
        private readonly IPathManager _pathManager;

        public KnowledgesController(IConfigRepository configRepository, IPathManager pathManager,
            IAuthManager authManager, IKnowlegesRepository knowlegesRepository, IKnowlegesTreeRepository knowlegesTreeRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _knowlegesRepository = knowlegesRepository;
            _knowlegesTreeRepository = knowlegesTreeRepository;
            _pathManager = pathManager;
        }
        public class GetRequest
        {
            public int TreeId { get; set; }
            public string Keywords { get; set; }
            public bool Like { get; set; }
            public bool Collect { get; set; }
            public string Orderby { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<Models.Knowledges> List { get; set; }
            public int Total { get; set; }
        }

        public class GetTreeResult
        {
            public int Total { get; set; }
            public List<GetTreeResultInfo> List { get; set; }
        }
        public class GetTreeResultInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
