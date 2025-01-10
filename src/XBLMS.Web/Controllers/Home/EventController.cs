using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class EventController : ControllerBase
    {
        private const string Route = "event";

        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;
        private readonly IExamManager _examManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;

        public EventController(IAuthManager authManager,
            IUserRepository userRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamManager examManager,
            IExamPaperRepository examPaperRepository)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
            _examPaperRepository = examPaperRepository;
        }
        public class GetRequest
        {
            public bool IsApp { get; set; }
        }

        public class GetResult
        {
            public List<GetResultEvent> List { get; set; }
        }
        public class GetResultEvent
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public List<string> ClassNames { get; set; }
        }
    }
}
