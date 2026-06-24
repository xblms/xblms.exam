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
    public partial class StudyPlanManagerController : ControllerBase
    {
        private const string Route = "study/studyPlanManager";
        private const string RouteCourse = Route + "/course";
        private const string RouteCourseExport = RouteCourse + "/export";
        private const string RouteUser = Route + "/user";
        private const string RouteUserExport = RouteUser + "/export";
        private const string RouteScore = Route + "/score";
        private const string RouteScoreExport = RouteScore + "/export";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamCerRepository _examCerRepository;

        public StudyPlanManagerController(IAuthManager authManager,
            IPathManager pathManager,
            IStudyManager studyManager,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IOrganManager organManager,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperRepository examPaperRepository,
            IExamCerRepository examCerRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _studyPlanRepository = studyPlanRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
            _organManager = organManager;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperRepository = examPaperRepository;
            _examCerRepository = examCerRepository;
        }
        public class GetResult
        {
            public StudyPlan Item { get; set; }
        }
        public class GetUserRequest
        {
            public int Id { get; set; }
            public string KeyWords { get; set; }
            public string State { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

        }
        public class GetUserResult
        {
            public int Total { get; set; }
            public List<StudyPlanUser> List { get; set; }
        }
        public class GetCourseRequest
        {
            public int Id { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetCourseResult
        {
            public string PlanName { get; set; }
            public List<StudyPlanCourse> List { get; set; }
        }


        public class GetSocreRequest
        {
            public int Id { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetScoreResult
        {
            public int Total { get; set; }
            public List<ExamPaperStart> List { get; set; }
        }

    }
}

