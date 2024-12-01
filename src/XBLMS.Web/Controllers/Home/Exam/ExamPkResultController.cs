using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamPkResultController : ControllerBase
    {
        private const string Route = "exam/examPkResult";

        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly ICacheManager _cacheManager;

        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;
        private readonly IExamPkUserRepository _examPkUserRepository;

        public ExamPkResultController(IAuthManager authManager,
            IOrganManager organManager,
            ICacheManager cacheManager,
            IExamPkRepository examPkRepository,
            IExamPkRoomRepository examPkRoomRepository,
            IExamPkUserRepository examPkUserRepository)
        {
            _authManager = authManager;
            _cacheManager = cacheManager;
            _organManager = organManager;
            _examPkRepository = examPkRepository;
            _examPkRoomRepository = examPkRoomRepository;
            _examPkUserRepository = examPkUserRepository;
        }


        public class GetRequest
        {
            public int Id { get; set; }
        }
        public class GetResult
        {
            public ExamPkRoom Room { get; set; }
            public string Title { get; set; }
        }
    }
}
