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
    public partial class ExamAssessmentConfigController : ControllerBase
    {
        private const string Route = "exam/examAssessmentConfig";
        private const string RouteDelete = Route + "/del";
        private const string RouteLock = Route + "/lock";
        private const string RouteUnLock = Route + "/unLock";

        private readonly IAuthManager _authManager;

        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;
        private readonly IExamAssessmentConfigSetRepository _examAssessmentConfigSetRepository;

        public ExamAssessmentConfigController(IAuthManager authManager,
            IExamManager examManager,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentConfigRepository examAssessmentConfigRepository,
            IExamAssessmentConfigSetRepository examAssessmentConfigSetRepository)
        {
            _authManager = authManager;
            _examAssessmentRepository = examAssessmentRepository;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
            _examAssessmentConfigSetRepository = examAssessmentConfigSetRepository;
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
            public List<ExamAssessmentConfig> List { get; set; }
            public int Total { get; set; }

        }
    }
}
