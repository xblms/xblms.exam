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
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamAssessmentingController : ControllerBase
    {
        private const string Route = "exam/examAssessmenting";
        private const string RouteSubmitPaper = Route + "/submitPaper";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;

        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentTmRepository _examAssessmentTmRepository;
        private readonly IExamAssessmentAnswerRepository _examAssessmentAnswerRepository;
        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;
        private readonly IExamAssessmentConfigSetRepository _examAssessmentConfigSetRepository;

        public ExamAssessmentingController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentTmRepository examAssessmentTmRepository,
            IExamAssessmentAnswerRepository examAssessmentAnswerRepository,
            IExamAssessmentConfigRepository examAssessmentConfigRepository,
            IExamAssessmentConfigSetRepository examAssessmentConfigSetRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examAssessmentRepository = examAssessmentRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentTmRepository = examAssessmentTmRepository;
            _examAssessmentAnswerRepository = examAssessmentAnswerRepository;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
            _examAssessmentConfigSetRepository = examAssessmentConfigSetRepository;
        }

        public class GetRequest
        {
            public int Id { get; set; }
            public string ps { get; set; }
        }

        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamAssessment Item { get; set; }
            public List<ExamAssessmentTm> TmList { get; set; }
        }

        public class GetSubmitRequest
        {
            public int Id { get; set; }
            public List<ExamAssessmentTm> TmList { get; set; }
        }
    }
}
