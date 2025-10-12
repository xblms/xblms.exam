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
    public partial class ExamPkUsersController : ControllerBase
    {
        private const string Route = "exam/examPkUsers";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;

        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamPkUserRepository  _examPkUserRepository;


        public ExamPkUsersController(IAuthManager authManager,
            IPathManager pathManager,
            IOrganManager organManager,
            IExamPkRepository examPkRepository,
            IExamPkUserRepository examPkUserRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _organManager = organManager;
            _examPkRepository = examPkRepository;
            _examPkUserRepository = examPkUserRepository;
        }


        public class GetUserRequest
        {
            public int Id { get; set; }
            public string KeyWords { get; set; }
        }
        public class GetUserResult
        {
            public string Title { get; set; }
            public int Total { get; set; }
            public List<ExamPkUser> List { get; set; }
        }
    }
}
