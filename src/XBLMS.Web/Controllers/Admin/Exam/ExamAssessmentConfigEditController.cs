using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamAssessmentConfigEditController : ControllerBase
    {
        private const string Route = "exam/examAssessmentConfigEdit";

        private readonly IAuthManager _authManager;

        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;
        private readonly IExamAssessmentConfigSetRepository _examAssessmentConfigSetRepository;

        public ExamAssessmentConfigEditController(IAuthManager authManager,
            IExamManager examManager,
            IExamAssessmentConfigRepository examAssessmentConfigRepository,
            IExamAssessmentConfigSetRepository examAssessmentConfigSetRepository)
        {
            _authManager = authManager;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
            _examAssessmentConfigSetRepository = examAssessmentConfigSetRepository;
        }
        public class GetRequest
        {
            public int Id { get; set; }
            public bool Face { get; set; }
        }
        public class GetResult
        {
            public ExamAssessmentConfig Item { get; set; }
            public List<ExamAssessmentConfigSet> ItemList { get; set; }
        }
        public class GetSubmitRequest
        {
            public ExamAssessmentConfig Item { get; set; }
            public List<ExamAssessmentConfigSet> ItemList { get; set; }
        }

    }
}

