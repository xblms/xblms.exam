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
    public partial class StudyCourseEvaluationSelectController : ControllerBase
    {
        private const string Route = "study/studyCourseEvaluationSelect";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;
        private readonly IStudyCourseEvaluationItemRepository _studyCourseEvaluationItemRepository;

        public StudyCourseEvaluationSelectController(IAuthManager authManager,
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
