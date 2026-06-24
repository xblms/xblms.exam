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

        private readonly IOrganManager _organManager;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;


        public ExamAssessmentUsersController(IOrganManager organManager,
            IExamAssessmentUserRepository examAssessmentUserRepository)
        {
            _organManager = organManager;
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
