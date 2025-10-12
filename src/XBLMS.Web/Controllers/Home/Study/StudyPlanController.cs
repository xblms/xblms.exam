using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class StudyPlanController : ControllerBase
    {
        private const string Route = "study/studyPlan";
        private const string RouteItem = Route + "/item";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;

        public StudyPlanController(IConfigRepository configRepository,
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
        public class GetRequest
        {
            public int Year { get; set; }
            public string State { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<StudyPlanUser> List { get; set; }
            public int Total { get; set; }
            public List<int> YearList { get; set; }
        }
    }
}
