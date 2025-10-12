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
        private readonly IStudyManager _studyManager;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;

        public StudyPlanMonthController(IAuthManager authManager,
            IStudyPlanRepository studyPlanRepository,
            IStudyManager studyManager,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseWareRepository studyCourseWareRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository)
        {
            _authManager = authManager;
            _studyManager = studyManager;
            _studyPlanRepository = studyPlanRepository;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseWareRepository = studyCourseWareRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
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
