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
    public partial class ExamPaperSelectController : ControllerBase
    {
        private const string Route = "exam/examPaperSelect";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        public ExamPaperSelectController(IAuthManager authManager,
            IExamPaperRepository examPaperRepository)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
        }

        public class GetRequest
        {
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamPaper> Items { get; set; }
            public int Total { get; set; }

        }
    }
}
