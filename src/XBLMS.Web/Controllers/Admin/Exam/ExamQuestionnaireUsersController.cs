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
    public partial class ExamQuestionnaireUsersController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaireUsers";

        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

        public ExamQuestionnaireUsersController(IAuthManager authManager,
            IOrganManager organManager,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
        {
            _authManager = authManager;
            _organManager = organManager;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
        }


        public class GetUserRequest
        {
            public int Id { get; set; }
            public string IsSubmit { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetUserResult
        {
            public int Total { get; set; }
            public List<ExamQuestionnaireUser> List { get; set; }
        }
    }
}
