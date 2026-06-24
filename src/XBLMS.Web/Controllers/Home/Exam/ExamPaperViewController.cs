using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPaperViewController : ControllerBase
    {
        private const string Route = "exam/examPaperView";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamManager _examManager;

        public ExamPaperViewController(IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository,
            IExamManager examManager,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamPaperStartRepository examPaperStartRepository)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _examManager = examManager;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examPaperStartRepository = examPaperStartRepository;
        }
        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamPaper Item { get; set; }
            public string TxList { get; set; }
            public string Salt { get; set; }
        }
    }
}
