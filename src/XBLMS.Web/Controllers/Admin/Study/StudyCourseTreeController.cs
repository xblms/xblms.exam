using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseTreeController : ControllerBase
    {
        private const string Route = "study/studyCourseTree";
        private const string RouteDelete = Route + "/del";
        private const string RouteAdd = Route + "/add";
        private const string RouteUpdate = Route + "/update";

        private readonly IAuthManager _authManager;
        private readonly IStudyCourseTreeRepository _studyCourseTreeRepository;
        private readonly IStudyManager _studyManager;

        public StudyCourseTreeController(IAuthManager authManager, IStudyCourseTreeRepository studyCourseTreeRepository, IStudyManager studyManager)
        {
            _authManager = authManager;
            _studyCourseTreeRepository = studyCourseTreeRepository;
            _studyManager = studyManager;
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
