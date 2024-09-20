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
    public partial class ExamPracticeResultController : ControllerBase
    {
        private const string Route = "exam/examPracticeResult";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPracticeRepository _examPracticeRepository;

        public ExamPracticeResultController(IConfigRepository configRepository,
            IAuthManager authManager, IExamPracticeRepository examPracticeRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPracticeRepository = examPracticeRepository;
        }
        public class GetResult
        {
            public int Total { get; set; }
            public int AnswerTotal { get; set; }
            public int RightTotal { get; set; }
            public int WrongTotal { get; set; }
        }
    }
}
