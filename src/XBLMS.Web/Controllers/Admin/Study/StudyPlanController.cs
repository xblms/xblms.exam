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
    public partial class StudyPlanController : ControllerBase
    {
        private const string Route = "study/studyPlan";
        private const string RouteDelete = Route + "/del";
        private const string RouteLock = Route + "/lock";
        private const string RouteUnLock = Route + "/unLock";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;

        public StudyPlanController(IAuthManager authManager,
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
        public class GetLockedRequest
        {
            public int Id { get; set; }
            public bool Locked { get; set; }
        }
        public class GetRequest
        {
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<StudyPlan> List { get; set; }
            public int Total { get; set; }

        }
    }
}
