using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPkController : ControllerBase
    {
        private const string Route = "exam/examPk";
        private const string RouteMyRoom = Route + "/rooms";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;

        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamPkUserRepository _examPkUserRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;

        public ExamPkController(IConfigRepository configRepository,
            IAuthManager authManager,
            IOrganManager organManager,
            IExamPkRepository examPkRepository,
            IExamPkUserRepository examPkUserRepository,
            IExamPkRoomRepository examPkRoomRepository,
            IUserRepository userRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _organManager = organManager;
            _examPkRepository = examPkRepository;
            _examPkUserRepository = examPkUserRepository;
            _examPkRoomRepository = examPkRoomRepository;
            _userRepository = userRepository;
        }

        public class GetRequest
        {
            public string KeyWords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamPk> List { get; set; }
            public int Total { get; set; }
        }
        public class GetMyRoomRequest
        {
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetMyRoomResult
        {
            public List<ExamPkRoom> List { get; set; }
            public int Total { get; set; }
        }
    }
}
