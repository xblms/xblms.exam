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
    public partial class ExamPaperMarkController : ControllerBase
    {
        private const string Route = "exam/examPaperMark";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;

        public ExamPaperMarkController(IAuthManager authManager,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperRepository examPaperRepository)
        {
            _authManager = authManager;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperRepository = examPaperRepository;
        }



        public class GetRequest
        {
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public int Total { get; set; }
            public List<ExamPaperStart> List { get; set; }
        }

    }
}
