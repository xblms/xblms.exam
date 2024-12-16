using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamPkEditDateController : ControllerBase
    {
        private const string Route = "exam/examPkEditDate";

        private readonly IAuthManager _authManager;
        private readonly IExamPkRepository  _examPkRepository;

        public ExamPkEditDateController(IAuthManager authManager,
            IExamPkRepository examPkRepository)
        {
            _authManager = authManager;
            _examPkRepository = examPkRepository;
        }
        public class GetResult
        {
            public ExamPk Item { get; set; }
        }
        public class GetSubmitRequest
        {
            public ExamPk Item { get; set; }
        }

    }
}

