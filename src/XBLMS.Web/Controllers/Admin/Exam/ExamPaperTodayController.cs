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
    public partial class ExamPaperTodayController : ControllerBase
    {
        private const string Route = "exam/examPaperToday";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;

        public ExamPaperTodayController(IAuthManager authManager,
            IExamPaperRepository examPaperRepository)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
        }
        public class GetRequest
        {
            public string DateType { get; set; }
        }
        public class GetResult
        {
            public List<ExamPaper> Items { get; set; }

        }
    }
}
