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
        private const string RouteUserExport = RouteUser + "/export";


        private const string RouteScore = Route + "/score";
        private const string RouteScoreExport = RouteScore + "/export";

        private const string RouteMark = Route + "/mark";


        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IOrganManager _organManager;
        private readonly IUserRepository _userRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;

        public ExamPaperManagerController(IAuthManager authManager,
            IPathManager pathManager,
            IOrganManager organManager,
            IUserRepository userRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamPaperRepository examPaperRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _organManager = organManager;
            _userRepository = userRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examPaperRepository = examPaperRepository;
        }
        public class GetResult
        {
            public string Title { get; set; }
            public int TotalScore { get; set; }
            public int PassScore { get; set; }
            public int TotalUser { get; set; }
            public int TotalExamTimes { get; set; }
            public int TotalExamTimesDistinct { get; set; }
            public int TotalPass { get; set; }
            public int TotalPassDistinct { get; set; }
            public decimal MaxScore { get; set; }
            public decimal MinScore { get; set; }
            public decimal TotalUserScore { get; set; }
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

        public class GetSocreRequest
        {
            public int Id { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetScoreResult
        {
            public int Total { get; set; }
            public List<ExamPaperStart> List { get; set; }
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
