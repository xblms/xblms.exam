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
    public partial class ExamAssessmentEditController : ControllerBase
    {
        private const string Route = "exam/examAssessmentEdit";
        private const string RouteUploadTm = Route + "/uploadTm";

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IPathManager _pathManager;
        private readonly ICreateManager _createManager;
        private readonly IUserGroupRepository _userGroupRepository;

        private readonly IExamAssessmentRepository _examAssessmentRepository;
        private readonly IExamAssessmentTmRepository _examAssessmentTmRepository;
        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentConfigRepository _examAssessmentConfigRepository;

       

        public ExamAssessmentEditController(IAuthManager authManager,
            IPathManager pathManager,
            IExamManager examManager,
            IUserGroupRepository userGroupRepository,
            IExamAssessmentRepository examAssessmentRepository,
            IExamAssessmentTmRepository examAssessmentTmRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentConfigRepository examAssessmentConfigRepository,
            ICreateManager createManager)
        {
            _authManager = authManager;
            _examManager = examManager;
            _pathManager = pathManager;
            _userGroupRepository = userGroupRepository;
            _examAssessmentRepository = examAssessmentRepository;
            _examAssessmentTmRepository = examAssessmentTmRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentConfigRepository = examAssessmentConfigRepository;
            _createManager = createManager;
        }
        public class GetUploadTmResult
        {
            public List<string> ErrorMsgList { get; set; }
            public List<ExamAssessmentTm> TmList { get; set; }
        }
        public class GetResult
        {
            public ExamAssessment Item { get; set; }
            public List<UserGroup> UserGroupList { get; set; }
            public List<ExamAssessmentConfig> ConfigList { get; set; }
            public List<ExamAssessmentTm> TmList { get; set; }
        }
        public class GetSubmitRequest
        {
            public SubmitType SubmitType { get; set; }
            public ExamAssessment Item { get; set; }
            public List<ExamAssessmentTm> TmList { get; set; }
        }

    }
}

