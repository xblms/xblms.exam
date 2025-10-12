using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UserDocLayerViewController : ControllerBase
    {
        private const string Route = "common/userDocLayerView";
        private const string RouteIndex = "common/userDocIndex";
        private const string RouteLoginLog = "common/userDocLoginLog";
        private const string RouteAss = "common/userDocAss";
        private const string RouteExam = "common/userDocExam";
        private const string RouteExamCer = "common/userDocExamCer";
        private const string RouteExamMoni = "common/userDocExamMoni";
        private const string RoutePointLog = "common/userDocPointLog";
        private const string RoutePointShopLog = "common/userDocPointShopLog";
        private const string RoutePractice = "common/userDocPractice";
        private const string RouteQ = "common/userDocQ";
        private const string RouteStudyCourse = "common/userDocStudyCourse";
        private const string RouteStudyPlan = "common/userDocStudyPlan";


        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IOrganManager _organManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IConfigRepository _configRepository;

        public UserDocLayerViewController(IAuthManager authManager, IUserRepository userRepository, IUserGroupRepository userGroupRepository, IOrganManager organManager, IDatabaseManager databaseManager, IConfigRepository configRepository)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _organManager = organManager;
            _databaseManager = databaseManager;
            _configRepository = configRepository;
        }

        public class GetRequest
        {
            public int Id { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }

        public class GetResult
        {
            public string DisplayName { get; set; }
            public string AvatarUrl { get; set; }
            public SystemCode SystemCode { get; set; }
        }
        public class GetIndexResult
        {
            public SystemCode SystemCode { get; set; }
            public string TotalCredit { get; set; }
            public string TotalDuration { get; set; }
            public string TotalPoints { get; set; }
            public string TotalCers { get; set; }
            public string TotalLogins { get; set; }

            public int PlanTotal { get; set; }
            public int PlanOverTotal { get; set; }
            public int PlanDabiaoTotal { get; set; }
            public int CourseTotal { get; set; }
            public int CourseOverTotal { get; set; }

            public int ExamTotal { get; set; }
            public int ExamPassTotal { get; set; }
            public int ExamMoniTotal { get; set; }
            public int ExamMoinPasaTotal { get; set; }

            public int ExamQTotal { get; set; }
            public int ExamQSubmitTotal { get; set; }

            public int ExamAssTotal { get; set; }
            public int ExamAssSubmitTotal { get; set; }

            public int ExamPracticeAnswerTotal { get; set; }
            public int ExamPracticeRightTotal { get; set; }
        }
    }
}
