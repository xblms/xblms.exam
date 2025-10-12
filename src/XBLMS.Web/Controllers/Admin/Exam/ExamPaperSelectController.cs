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
        private readonly IExamManager _examManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperTreeRepository _examPaperTreeRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        public ExamPaperSelectController(IAuthManager authManager,
            IExamManager examManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperTreeRepository examPaperTreeRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _examPaperRepository = examPaperRepository;
            _examPaperTreeRepository = examPaperTreeRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
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
