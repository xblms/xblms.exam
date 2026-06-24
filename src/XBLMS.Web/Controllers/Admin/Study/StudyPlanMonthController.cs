using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyPlanMonthController : ControllerBase
    {
        private const string Route = "study/studyPlanMonth";

        private readonly IAuthManager _authManager;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;

        public StudyPlanMonthController(IAuthManager authManager,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanUserRepository studyPlanUserRepository)
        {
            _authManager = authManager;
            _studyPlanRepository = studyPlanRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
        }
        public class GetRequest
        {
            public bool IsOver { get; set; }
        }
        public class GetResult
        {
            public List<StudyPlan> List { get; set; }
            public int Total { get; set; }

        }
    }
}
