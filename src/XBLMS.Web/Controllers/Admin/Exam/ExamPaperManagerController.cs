using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamPaperManagerController : ControllerBase
    {
        private const string Route = "exam/examPaperManager";

        private const string RouteUser = Route + "/user";
        private const string RouteUserUpdateDateTime = RouteUser + "/datetime";
        private const string RouteUserUpdateExamTimes = RouteUser + "/examtimes";
        private const string RouteUserDelete = RouteUser + "/remove";
        private const string RouteUserDeleteOne = RouteUser + "/removeone";


        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;

        public ExamPaperManagerController(IAuthManager authManager,
            IOrganManager organManager,
            IUserRepository userRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamPaperStartRepository examPaperStartRepository)
        {
            _authManager = authManager;
            _organManager = organManager;
            _userRepository = userRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examPaperStartRepository = examPaperStartRepository;
        }
        public class GetUserRequest
        {
            public int Id { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetUserResult
        {
            public int Total { get; set; }
            public List<ExamPaperUser> List { get; set; }
        }


        public class GetUserUpdateRequest
        {
            public int Id { get; set; }
            public List<int> Ids { get; set; }
            public bool Increment { get; set; }
            public DateTime ExamBeginDateTime { get; set; }
            public DateTime ExamEndDateTime { get; set; }
        }
    }
}
