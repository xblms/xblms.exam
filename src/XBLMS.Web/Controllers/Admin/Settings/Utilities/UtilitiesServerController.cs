using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Utilities
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class UtilitiesServerController : ControllerBase
    {
        private const string Route = "settings/serverConfig";

        private const string RouteAI = Route + "/ai";
        private const string RouteAITest = RouteAI + "/test";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAiTaskService _aiTaskService;

        public UtilitiesServerController(ISettingsManager settingsManager, IAuthManager authManager, IConfigRepository configRepository, IAiTaskService aiTaskService)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _configRepository = configRepository;
            _aiTaskService = aiTaskService;
        }

        public class GetItem
        {
            public SystemCode SystemCode { get; set; } = SystemCode.Exam;
            public string SystemCodeName { get; set; }
            public string AiHostUrl { get; set; }
            public bool AiServe { get; set; }
            public string AiRunningModel { get; set; }
            public bool IsModels { get; set; }
            public List<DoAI.DoAI_RunningModels_Result_Info> AiRunningModels { get; set; }
        }

        public class GetAIRequest
        {
            public string AiHostUrl { get; set; }
            public bool AiServe { get; set; }
            public string AiRunningModel { get; set; }
        }
        public class GetAIVersionResult
        {
            public bool Success { get; set; }
            public string Msg { get; set; }
            public bool IsModels { get; set; }
            public List<DoAI.DoAI_RunningModels_Result_Info> Models { get; set; }
        }
    }
}
