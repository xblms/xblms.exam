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
    public partial class StudyOffCourseWeekController : ControllerBase
    {
        private const string Route = "study/studyOffCourseWeek";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;

        public StudyOffCourseWeekController(IAuthManager authManager,
            IStudyManager studyManager,
            IStudyCourseRepository studyCourseRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyPlanRepository studyPlanRepository)
        {
            _authManager = authManager;
            _studyManager = studyManager;
            _studyCourseRepository = studyCourseRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _studyPlanRepository = studyPlanRepository;
        }
        public class GetResult
        {
            public List<StudyCourse> List { get; set; }
            public int Total { get; set; }

        }
    }
}
