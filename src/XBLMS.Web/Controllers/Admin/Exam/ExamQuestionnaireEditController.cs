using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamQuestionnaireEditController : ControllerBase
    {
        private const string Route = "exam/examQuestionnaireEdit";
        private const string RouteUploadTm = Route + "/uploadTm";

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IPathManager _pathManager;
        private readonly IUserGroupRepository _userGroupRepository;

        private readonly IExamQuestionnaireRepository _questionnaireRepository;
        private readonly IExamQuestionnaireTmRepository _questionnaireTmRepository;
        private readonly IExamQuestionnaireUserRepository _questionnaireUserRepository;
       


        public ExamQuestionnaireEditController(IAuthManager authManager,
            IPathManager pathManager,
            IExamManager examManager,
            IUserGroupRepository userGroupRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository)
        {
            _authManager = authManager;
            _examManager = examManager;
            _pathManager= pathManager;
            _userGroupRepository = userGroupRepository;
            _questionnaireRepository = examQuestionnaireRepository;
            _questionnaireTmRepository = examQuestionnaireTmRepository;
            _questionnaireUserRepository = examQuestionnaireUserRepository;
        }
        public class GetUploadTmResult
        {
            public List<string> ErrorMsgList { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }
        public class GetResult
        {
            public ExamQuestionnaire Item { get; set; }
            public List<UserGroup> UserGroupList { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }
        public class GetSubmitRequest
        {
            public SubmitType SubmitType { get; set; }
            public ExamQuestionnaire Item { get; set; }
            public List<ExamQuestionnaireTm> TmList { get; set; }
        }

    }
}

