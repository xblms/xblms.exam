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
    public partial class ExamPaperInfoController : ControllerBase
    {
        private const string Route = "exam/examPaperInfo";
        private const string RouteCheck = Route + "/check";
        private const string RouteClientExam = Route + "/clientExam";
        private const string RouteClientExamStatus = Route + "/clientExamStatus";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamManager _examManager;

        public ExamPaperInfoController(IConfigRepository configRepository, IAuthManager authManager, IExamPaperRepository examPaperRepository, IExamPaperUserRepository examPaperUserRepository, IExamManager examManager)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
        }
        public class GetResult
        {
            public ExamPaper Item { get; set; }
        }
        public class GetCheckResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
        }
        public class GetClientUrlRequest
        {
            public int Id { get; set; }
            public string Token { get; set; }
        }
    }
}
