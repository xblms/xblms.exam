using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Knowledges
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class KnowledgesViewController : ControllerBase
    {
        private const string Route = "knowledgesView";
        private const string RouteLike = Route + "/like";
        private const string RouteCollect = Route + "/collect";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IKnowlegesRepository _knowlegesRepository;
        private readonly IPathManager _pathManager;

        public KnowledgesViewController(IConfigRepository configRepository,
            IAuthManager authManager, IKnowlegesRepository knowlegesRepository, IPathManager pathManager)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _knowlegesRepository = knowlegesRepository;
            _pathManager = pathManager;
        }

        public class GetResult
        {
            public PointNotice PointNotice { get; set; }
            public string Name { get; set; }
            public bool IsLike { get; set; }
            public bool IsCollect { get; set; }
            public int Likes { get; set; }
            public int Collects { get; set; }
            public string Url { get; set; }
        }
    }
}
