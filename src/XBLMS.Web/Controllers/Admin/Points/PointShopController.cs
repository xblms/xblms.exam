using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Points
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UtilitiesPointShopController : ControllerBase
    {
        private const string Route = "points/pointshop";
        private const string RouteDelete = Route + "/del";

        private const string RouteItem = Route + "/item";
        private const string RouteItemUpload = RouteItem + "/upload";

        private const string RouteUsers = Route + "/users";

        private const string RouteConfig = Route + "/config";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IPointShopRepository _pointShopRepository;
        private readonly IUploadManager _uploadManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IPointShopUserRepository _pointShopUserRepository;
        private readonly IUserRepository _userRepository;

        public UtilitiesPointShopController(ISettingsManager settingsManager, IAuthManager authManager, IConfigRepository configRepository, IPointShopRepository pointShopRepository, IUploadManager uploadManager, IAdministratorRepository administratorRepository, IPointShopUserRepository pointShopUserRepository, IUserRepository userRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _configRepository = configRepository;
            _pointShopRepository = pointShopRepository;
            _uploadManager = uploadManager;
            _administratorRepository = administratorRepository;
            _pointShopUserRepository = pointShopUserRepository;
            _userRepository = userRepository;
        }


        public class GetRequest
        {
            public int Id { get; set; }
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public int Total { get; set; }
            public List<PointShop> List { get; set; }
        }
        public class GetItem
        {
            public PointShop Item { get; set; }
            public List<GetUploadResult> FileList { get; set; }
        }

        public class GetUploadResult
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class GetUsersResult
        {
            public int Total { get; set; }
            public List<PointShopUser> List { get; set; }
        }

        public class GetSetStateRequest
        {
            public int Id { get; set; }
            public PointShopState State { get; set; }
        }

        public class GetPointConfig
        {
            public int PointLogin { get; set; }
            public int PointLoginDayMax { get; set; }
            public int PointPlanOver { get; set; }
            public int PointPlanOverDayMax { get; set; }
            public int PointVideo { get; set; }
            public int PointVideoDayMax { get; set; }
            public int PointDocument { get; set; }
            public int PointDocumentDayMax { get; set; }
            public int PointCourseOver { get; set; }
            public int PointCourseOverDayMax { get; set; }
            public int PointEvaluation { get; set; }
            public int PointEvaluationDayMax { get; set; }
            public int PointExam { get; set; }
            public int PointExamDayMax { get; set; }
            public int PointExamPass { get; set; }
            public int PointExamPassDayMax { get; set; }
            public int PointExamFull { get; set; }
            public int PointExamFullDayMax { get; set; }
            public int PointExamQ { get; set; }
            public int PointExamQDayMax { get; set; }
            public int PointExamAss { get; set; }
            public int PointExamAssDayMax { get; set; }
            public int PointExamPractice { get; set; }
            public int PointExamPracticeDayMax { get; set; }
            public int PointExamPracticeRight { get; set; }
            public int PointExamPracticeRightDayMax { get; set; }
            public SystemCode SystemCode { get; set; }
        }
    }
}
