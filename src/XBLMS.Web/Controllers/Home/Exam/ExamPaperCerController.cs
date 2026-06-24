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

        private readonly IAuthManager _authManager;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;
        private readonly IExamCerRepository _examCerRepository;

        public ExamPaperCerController(IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamCerRepository examCerRepository)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
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
