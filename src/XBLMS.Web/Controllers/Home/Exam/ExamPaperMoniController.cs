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
    public partial class ExamPaperMoniController : ControllerBase
    {
        private const string Route = "exam/examPaperMoni";
        private const string RouteItem = Route + "/item";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamManager _examManager;

        public ExamPaperMoniController(IConfigRepository configRepository, IAuthManager authManager, IExamPaperRepository examPaperRepository, IExamPaperUserRepository examPaperUserRepository, IExamManager examManager)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
        }
        public class GetRequest
        {
            public bool IsApp { get; set; }
            public string KeyWords { get; set; }
            public string Date { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamPaper> List { get; set; }
            public int Total { get; set; }
        }
    }
}
