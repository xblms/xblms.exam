using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
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

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly ICreateManager _createManager;

        public ExamPaperSubmitResultController(IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperStartRepository examPaperStartRepository,
            ICreateManager createManager)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
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
