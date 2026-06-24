using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class StudyPlanInfoController : ControllerBase
    {
        private const string Route = "study/studyPlanInfo";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;

        public StudyPlanInfoController(IAuthManager authManager,
            IStudyManager studyManager,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanUserRepository studyPlanUserRepository)
        {
            _authManager = authManager;
            _studyManager = studyManager;
            _studyPlanRepository = studyPlanRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
        }
        public class GetResult
        {
            public PointNotice PointNotice { get; set; }
            public StudyPlanUser Item { get; set; }
        }
    }
}
