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
    public partial class ExamAssessmentController : ControllerBase
    {
        private const string Route = "exam/examAssessment";
        private const string RouteDelete = Route + "/del";
        private const string RouteLock = Route + "/lock";
        private const string RouteUnLock = Route + "/unLock";

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;

        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;

        public ExamAssessmentController(IAuthManager authManager,
            IExamManager examManager,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentConfigRepository examAssessmentConfigRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _examAssessmentRepository = examAssessmentRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
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
            public List<ExamAssessment> List { get; set; }
            public int Total { get; set; }

        }
    }
}
