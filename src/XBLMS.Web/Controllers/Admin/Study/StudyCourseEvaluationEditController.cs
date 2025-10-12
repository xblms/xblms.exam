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
    public partial class StudyCourseEvaluationEditController : ControllerBase
    {
        private const string Route = "study/studyCourseEvaluationEdit";
        private const string RouteUpload = Route + "/upload";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;
        private readonly IStudyCourseEvaluationItemRepository _studyCourseEvaluationItemRepository;

        public StudyCourseEvaluationEditController(IAuthManager authManager,
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
            public int Id { get; set; }
            public bool Face { get; set; }
        }
        public class GetResult
        {
            public StudyCourseEvaluation Item { get; set; }
            public List<StudyCourseEvaluationItem> ItemList { get; set; }
        }
        public class GetSubmitRequest
        {
            public StudyCourseEvaluation Item { get; set; }
            public List<StudyCourseEvaluationItem> ItemList { get; set; }
        }

    }
}

