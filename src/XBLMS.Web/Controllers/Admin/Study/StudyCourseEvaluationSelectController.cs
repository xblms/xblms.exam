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
        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;

        public StudyCourseEvaluationSelectController(IAuthManager authManager,
            IStudyCourseEvaluationRepository studyCourseEvaluationRepository)
        {
            _authManager = authManager;
            _studyCourseEvaluationRepository = studyCourseEvaluationRepository;
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
