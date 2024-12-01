using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamPkController : ControllerBase
    {
        private const string Route = "exam/examPk";
        private const string RouteDelete = Route + "/del";

        private readonly IAuthManager _authManager;
        private readonly IExamPkRepository _examPkRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;
        private readonly IExamPkRoomAnswerRepository _examPkRoomAnswerRepository;
        private readonly IExamPkUserRepository _examPkUserRepository;

        public ExamPkController(IAuthManager authManager,
            IExamPkRepository examPkRepository,
            IExamPkRoomRepository examPkRoomRepository,
            IExamPkRoomAnswerRepository examPkRoomAnswerRepository,
            IExamPkUserRepository examPkUserRepository)
        {
            _authManager = authManager;
            _examPkRepository = examPkRepository;
            _examPkRoomRepository = examPkRoomRepository;
            _examPkRoomAnswerRepository = examPkRoomAnswerRepository;
            _examPkUserRepository = examPkUserRepository;
        }
        public class GetRequest
        {
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<ExamPk> List { get; set; }
            public int Total { get; set; }

        }
    }
}
