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
    public partial class ExamPaperSubmitResultController : ControllerBase
    {
        private const string Route = "exam/examPaperSubmitResult";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamManager _examManager;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly ICreateManager _createManager;

        public ExamPaperSubmitResultController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamManager examManager,
            IExamPaperStartRepository examPaperStartRepository,
            ICreateManager createManager)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
            _examPaperStartRepository = examPaperStartRepository;
            _createManager = createManager;
        }
        public class GetResult
        {
            public int Queue { get; set; }
            public bool Success { get; set; }
            public bool IsPass { get; set; }
            public decimal Score { get; set; }
            public bool IsShowScore { get; set; }
            public string Title { get; set; }

            public bool IsMark { get; set; }
        }
    }
}
