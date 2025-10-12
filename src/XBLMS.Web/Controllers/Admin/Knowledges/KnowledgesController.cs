using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class KnowledgesController : ControllerBase
    {
        private const string Route = "knowledges";

        private const string RouteDelete = Route + "/del";
        private const string RouteLock = Route + "/lock";
        private const string RouteUnLock = Route + "/unLock";
        private const string RouteSubmit = Route + "/submit";
        private const string RouteUpload = Route + "/upload";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly ISettingsManager _settingsManager;
        private readonly IKnowlegesRepository _knowlegesRepository;
        private readonly IKnowlegesTreeRepository _knowlegesTreeRepository;

        public KnowledgesController(IPathManager pathManager, IAuthManager authManager, ISettingsManager settingsManager, IKnowlegesRepository knowlegesRepository, IKnowlegesTreeRepository knowlegesTreeRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _settingsManager = settingsManager;
            _knowlegesRepository = knowlegesRepository;
            _knowlegesTreeRepository = knowlegesTreeRepository;
        }

        public class GetRequest
        {
            public int TreeId { get; set; }
            public bool TreeIsChildren { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public bool IsAdmin { get; set; }
            public int Total { get; set; }
            public List<Models.Knowledges> List { get; set; }
        }

        public class GetSubmitRequest
        {
            public int TreeId { get; set; }
            public List<Models.Knowledges> List { get; set; }
        }

        public class GetUploadResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string CoverImagePath { get; set; }
        }
    }
}
