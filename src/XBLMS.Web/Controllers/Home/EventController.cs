﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
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
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;

        public EventController(IAuthManager authManager,
            IUserRepository userRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamManager examManager,
            IExamPaperRepository examPaperRepository,
            IStudyCourseUserRepository studyCourseUserRepository,
            IStudyCourseRepository studyCourseRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository)
        {
            _authManager = authManager;
            _userRepository = userRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examManager = examManager;
            _examPaperRepository = examPaperRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
            _studyCourseRepository = studyCourseRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
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
            public int allow { get; set; } = 0;
            public string GroupId { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public List<string> ClassNames { get; set; }
        }
    }
}
