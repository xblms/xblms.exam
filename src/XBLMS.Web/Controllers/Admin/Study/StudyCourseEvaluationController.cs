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
    public partial class StudyCourseEvaluationController : ControllerBase
    {
        private const string Route = "study/studyCourseEvaluation";
        private const string RouteDelete = Route + "/del";
        private const string RouteLock = Route + "/lock";
        private const string RouteUnLock = Route + "/unLock";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;
        private readonly IStudyCourseEvaluationItemRepository _studyCourseEvaluationItemRepository;

        public StudyCourseEvaluationController(IAuthManager authManager,
            IExamManager examManager,
            IStudyManager studyManager,
            IStudyCourseEvaluationRepository studyCourseEvaluationRepository,
            IStudyCourseEvaluationItemRepository studyCourseEvaluationItemRepository)
        {
            _authManager = authManager;
            _studyManager = studyManager;
            _studyCourseEvaluationRepository = studyCourseEvaluationRepository;
            _studyCourseEvaluationItemRepository = studyCourseEvaluationItemRepository;
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
            public List<StudyCourseEvaluation> List { get; set; }
            public int Total { get; set; }

        }
    }
}
