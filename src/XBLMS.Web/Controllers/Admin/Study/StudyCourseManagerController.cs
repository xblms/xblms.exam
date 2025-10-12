using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseManagerController : ControllerBase
    {
        private const string Route = "study/studyCourseManager";

        private const string RouteCourse = Route + "/course";
        private const string RouteCourseExport = RouteCourse + "/export";

        private const string RouteUser = Route + "/user";
        private const string RouteUserExport = RouteUser + "/export";
        private const string RouteUserOfflineSet = RouteUser + "/set";
        private const string RouteUserOfflineOver = RouteUser + "/over";

        private const string RouteScore = Route + "/score";
        private const string RouteScoreExport = RouteScore + "/export";

        private const string RouteQ = Route + "/examq";
        private const string RouteQExport = RouteQ + "/export";

        private const string RouteEvaluation = Route + "/evaluation";
        private const string RouteEvaluationExport = RouteEvaluation + "/export";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IPathManager _pathManager;
        private readonly IUploadManager _uploadManager;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseTreeRepository _studyCourseTreeRepository;
        private readonly IStudyCourseFilesRepository _studyCourseFilesRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamCerRepository _examCerRepository;

        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireTmRepository _examQuestionnaireTmRepository;
        private readonly IExamQuestionnaireAnswerRepository _examQuestionnaireAnswerRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;

        private readonly IStudyCourseEvaluationRepository _studyCourseEvaluationRepository;
        private readonly IStudyCourseEvaluationItemRepository _studyCourseEvaluationItemRepository;
        private readonly IStudyCourseEvaluationItemUserRepository _studyCourseEvaluationItemUserRepository;
        private readonly IStudyCourseEvaluationUserRepository _studyCourseEvaluationUserRepository;

        public StudyCourseManagerController(IAuthManager authManager,
            IPathManager pathManager,
            IUploadManager uploadManager,
            IStudyManager studyManager,
            IUserGroupRepository userGroupRepository,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseFilesRepository studyCourseFilesRepository,
            IStudyCourseTreeRepository studyCourseTreeRepository,
            IStudyCourseWareRepository studyCourseWareRepository,
            IStudyPlanUserRepository studyPlanUserRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IOrganManager organManager,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperRepository examPaperRepository,
            IExamCerRepository examCerRepository,
            IUserRepository userRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireTmRepository examQuestionnaireTmRepository,
            IExamQuestionnaireAnswerRepository examQuestionnaireAnswerRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IStudyCourseEvaluationRepository studyCourseEvaluationRepository,
            IStudyCourseEvaluationItemRepository studyCourseEvaluationItemRepository,
            IStudyCourseEvaluationItemUserRepository studyCourseEvaluationItemUserRepository,
            IStudyCourseEvaluationUserRepository studyCourseEvaluationUserRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _uploadManager = uploadManager;
            _studyManager = studyManager;
            _userGroupRepository = userGroupRepository;
            _studyPlanRepository = studyPlanRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseTreeRepository = studyCourseTreeRepository;
            _studyCourseFilesRepository = studyCourseFilesRepository;
            _studyCourseWareRepository = studyCourseWareRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
            _organManager = organManager;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperRepository = examPaperRepository;
            _examCerRepository = examCerRepository;
            _userRepository = userRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireTmRepository = examQuestionnaireTmRepository;
            _examQuestionnaireAnswerRepository = examQuestionnaireAnswerRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _studyCourseEvaluationRepository = studyCourseEvaluationRepository;
            _studyCourseEvaluationItemRepository = studyCourseEvaluationItemRepository;
            _studyCourseEvaluationItemUserRepository = studyCourseEvaluationItemUserRepository;
            _studyCourseEvaluationUserRepository = studyCourseEvaluationUserRepository;
        }
        public class GetRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }
        }
        public class GetResult
        {
            public StudyCourse Item { get; set; }
        }
        public class GetUserRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }
            public string KeyWords { get; set; }
            public string State { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

        }
        public class GetUserResult
        {
            public int Total { get; set; }
            public List<StudyCourseUser> List { get; set; }
            public StudyCourse Course { get; set; }
        }
        public class GetExamqRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }

        }
        public class GetExamqResult
        {
            public int QTmTotal { get; set; }
            public int QAnswerTotal { get; set; }
            public List<ExamQuestionnaireTm> QList { get; set; }
        }


        public class GetSocreRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetScoreResult
        {
            public int Total { get; set; }
            public List<ExamPaperStart> List { get; set; }
        }




        public class GetEvaluationRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }

        }
        public class GetEvaluationResult
        {
            public int Total { get; set; }
            public List<StudyCourseEvaluationUser> List { get; set; }
            public List<StudyCourseEvaluationItem> Items { get; set; }
        }



        public class GetSetOfflineRequest
        {
            public string State { get; set; }
            public string KeyWords { get; set; }
            public int CourseId { get; set; }
            public int PlanId { get; set; }
            public List<int> CourseUserIds { get; set; }

        }

    }
}

