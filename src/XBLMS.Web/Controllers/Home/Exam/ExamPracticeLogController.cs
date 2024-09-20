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
    public partial class ExamPracticeLogController : ControllerBase
    {
        private const string Route = "exam/examPracticeLog";
        private const string RouteDelete = Route + "/del";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPracticeRepository _examPracticeRepository;

        public ExamPracticeLogController(IConfigRepository configRepository, IAuthManager authManager, IExamPracticeRepository examPracticeRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPracticeRepository = examPracticeRepository;
        }
        public class GetRequest
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamPractice> List { get; set; }
            public int Total { get; set; }
        }
    }
}
