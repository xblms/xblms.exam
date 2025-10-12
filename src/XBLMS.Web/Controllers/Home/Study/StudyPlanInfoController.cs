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

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;

        public StudyPlanInfoController(IConfigRepository configRepository,
            IAuthManager authManager,
            IStudyManager studyManager,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanUserRepository studyPlanUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _studyManager = studyManager;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
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
