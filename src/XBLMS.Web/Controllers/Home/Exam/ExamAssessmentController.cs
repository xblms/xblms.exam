using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamAssessmentController : ControllerBase
    {
        private const string Route = "exam/examAssessment";
        private const string RouteItem = "exam/examAssessment/item";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamManager _examManager;

        public ExamAssessmentController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamManager examManager,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examManager = examManager;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentRepository = examAssessmentRepository;
        }
        public class GetRequest
        {
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamAssessment> List { get; set; }
            public int Total { get; set; }
        }
    }
}
