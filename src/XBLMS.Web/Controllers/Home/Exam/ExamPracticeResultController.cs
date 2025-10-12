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
    public partial class ExamPracticeResultController : ControllerBase
    {
        private const string Route = "exam/examPracticeResult";
        private const string RouteView = Route + "/view";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPracticeRepository _examPracticeRepository;
        private readonly IExamPracticeAnswerRepository _examPracticeAnswerRepository;
        private readonly IExamManager _examManager;

        public ExamPracticeResultController(IConfigRepository configRepository,IExamManager examManager,
            IAuthManager authManager, IExamPracticeRepository examPracticeRepository, IExamPracticeAnswerRepository examPracticeAnswerRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examManager = examManager;
            _examPracticeRepository = examPracticeRepository;
            _examPracticeAnswerRepository = examPracticeAnswerRepository;
        }
        public class GetResult
        {
            public string Title { get; set; }
            public int Total { get; set; }
            public int AnswerTotal { get; set; }
            public int RightTotal { get; set; }
            public int WrongTotal { get; set; }
        }
        public class GetViewResult
        {
            public ExamPractice Item { get; set; }
            public string TmList { get; set; }
            public string Salt { get; set; }
        }
    }
}
