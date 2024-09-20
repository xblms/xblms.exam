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
    public partial class ExamPaperCerController : ControllerBase
    {
        private const string Route = "exam/examPaperCer";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamManager _examManager;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamCerRepository _examCerRepository;

        public ExamPaperCerController(IConfigRepository configRepository,
            IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamManager examManager,
            IExamPaperStartRepository examPaperStartRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamCerRepository examCerRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
            _examPaperStartRepository = examPaperStartRepository;
            _examCerUserRepository = examCerUserRepository;
            _examCerRepository = examCerRepository;
        }
        public class GetRequest
        {
            public string KeyWords { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
        }
        public class GetResult
        {
            public List<ExamCerUser> List { get; set; }
        }
    }
}
