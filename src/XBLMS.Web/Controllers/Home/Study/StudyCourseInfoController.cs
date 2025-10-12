using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
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
    public partial class StudyCourseInfoController : ControllerBase
    {
        private const string Route = "study/studyCourseInfo";
        private const string RouteWareSetProgress = Route + "/setProgress";
        private const string RouteWareSetOver = Route + "/setOver";

        private const string RouteCollection = Route + "/collection";
        private const string RouteEvaluation = Route + "/evaluation";

        private readonly IConfigRepository _configRepository;
        private readonly IPathManager _pathManager;
        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IOrganManager _organManager;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;
        private readonly IStudyCourseWareUserRepository _studyCourseWareUserRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IStudyCourseEvaluationUserRepository _studyCourseEvaluationUserRepository;
        private readonly IStudyCourseEvaluationItemUserRepository _studyCourseEvaluationItemUserRepository;
        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;

        public StudyCourseInfoController(IConfigRepository configRepository,
            IPathManager pathManager,
            IAuthManager authManager,
            IStudyManager studyManager,
            IOrganManager organManager,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IStudyCourseWareRepository studyCourseWareRepository,
            IStudyCourseWareUserRepository studyCourseWareUserRepository,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IExamPaperRepository examPaperRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IStudyCourseEvaluationUserRepository studyCourseEvaluationUserRepository,
            IStudyCourseEvaluationItemUserRepository studyCourseEvaluationItemUserRepository,
            IStudyCourseEvaluationRepository studyCourseEvaluationRepository,
            IStudyPlanUserRepository studyPlanUserRepository)
        {
            _configRepository = configRepository;
            _pathManager = pathManager;
            _authManager = authManager;
            _studyManager = studyManager;
            _organManager = organManager;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
            _studyCourseWareRepository = studyCourseWareRepository;
            _studyCourseWareUserRepository = studyCourseWareUserRepository;
            _studyPlanRepository = studyPlanRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _examPaperRepository = examPaperRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _studyCourseEvaluationUserRepository = studyCourseEvaluationUserRepository;
            _studyCourseEvaluationItemUserRepository = studyCourseEvaluationItemUserRepository;
            _studyCourseEvaluationRepository = studyCourseEvaluationRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
        }
        public class GetRequest
        {
            public int PlanId { get; set; }
            public int CourseId { get; set; }
        }
        public class GetResult
        {
            public PointNotice PointNotice { get; set; }
            public StudyCourse CourseInfo { get; set; }
        }


        public class GetWareSetProgressRequest
        {
            public int Id { get; set; }
            public int Progress { get; set; }
            public bool IsAdd { get; set; }
            public int CurrentDuration { get; set; }
        }
        public class GetWareSetProgressResult
        {
            public PointNotice PointNotice { get; set; }
            public long TotalDuration { get; set; }
            public bool StudyOver { get; set; }
        }
        public class GetEvaluationRequest
        {
            public int CourseId { get; set; }
            public int PlanId { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetEvaluationResult
        {
            public int Total { get; set; }
            public List<GetEvaluationResultInfo> List { get; set; }
            public List<int> StarList { get; set; }
            public string StarAvg { get; set; }
            public int StarUser { get; set; }

        }
        public class GetEvaluationResultInfo
        {
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public decimal Star { get; set; }
            public int MaxStar { get; set; }
            public string TextContent { get; set; }
            public DateTime StarDateTime { get; set; }
        }
    }
}
