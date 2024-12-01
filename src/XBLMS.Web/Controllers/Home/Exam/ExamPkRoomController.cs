using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPkRoomController : ControllerBase
    {
        private const string Route = "exam/examPkRoom";
        private const string RouteNotice = Route + "/notice";
        private const string RouteFinish = Route + "/finish";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;

        private readonly ISignalRHubManagerMessage _signalRHubManagerMessage;

        private readonly IExamManager _examManager;
        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;


        public ExamPkRoomController(IAuthManager authManager,
            ISettingsManager settingsManager,
            ICacheManager cacheManager,
            IOrganManager organManager,
            IUserRepository userRepository,
            ISignalRHubManagerMessage signalRHubManagerMessage,
            IExamManager examManager,
            IExamPkRepository examPkRepository,
            IExamPkRoomRepository examPkRoomRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _cacheManager = cacheManager;
            _organManager = organManager;
            _signalRHubManagerMessage = signalRHubManagerMessage;
            _userRepository = userRepository;
            _examManager = examManager;
            _examPkRepository = examPkRepository;
            _examPkRoomRepository = examPkRoomRepository;
        }


        public class GetRequest
        {
            public int Id { get; set; }
        }
        public class GetResult
        {
            public bool IsSafeMode { get; set; }
            public PkRoomResult CacheRoom { get; set; }
            public int UserId { get; set; }
            public List<ExamTm> TmList { get; set; }
        }
    }
}
