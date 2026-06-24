using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class StudyCourseLogController : ControllerBase
    {
        private const string Route = "study/studyCourseLog";
        private const string RouteItem = Route + "/item";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;
        public StudyCourseLogController(IAuthManager authManager,
            IStudyManager studyManager,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseUserRepository studyCourseUserRepository)
        {
            _authManager = authManager;
            _studyManager = studyManager;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
        }
        public class GetRequest
        {
            public string KeyWords { get; set; }
            public string Mark { get; set; }
            public string State { get; set; }
            public bool Collection { get; set; } 
            public string Orderby { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<string> MarkList { get; set; }
            public int MarkTotal { get; set; }
            public List<StudyCourse> List { get; set; }
            public int Total { get; set; }
        }
        public class GetItemRequest
        {
            public int Id { get; set; }
            public int PlanId { get; set; }
        }
    }
}
