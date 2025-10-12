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
    public partial class ExamAssessmentUsersController : ControllerBase
    {
        private const string Route = "exam/examAssessmentUsers";

        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;

        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;


        public ExamAssessmentUsersController(IAuthManager authManager,
            IPathManager pathManager,
            IOrganManager organManager,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _organManager = organManager;
            _examAssessmentRepository = examAssessmentRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
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
            public List<ExamAssessmentUser> List { get; set; }
        }
    }
}
