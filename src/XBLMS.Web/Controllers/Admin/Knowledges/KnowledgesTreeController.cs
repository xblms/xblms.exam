using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Knowledges
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class KnowledgesTreeController : ControllerBase
    {
        private const string Route = "knowledgesTree";
        private const string RouteDelete = Route + "/del";
        private const string RouteAdd = Route + "/add";
        private const string RouteUpdate = Route + "/update";

        private readonly IAuthManager _authManager;
        private readonly IKnowlegesTreeRepository _knowlegesTreeRepository;
        private readonly IExamManager _examManager;

        public KnowledgesTreeController(IAuthManager authManager, IExamManager examManager, IKnowlegesTreeRepository knowlegesTreeRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _knowlegesTreeRepository = knowlegesTreeRepository;
        }

        public class GetResult
        {
            public List<Cascade<int>> Items { get; set; }
        }
        public class GetTreeNamesRequest
        {
            public string Names { get; set; }
            public int ParentId { get; set; }
        }
    }
}
