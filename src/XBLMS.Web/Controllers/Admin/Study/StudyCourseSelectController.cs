using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseSelectController : ControllerBase
    {
        private const string Route = "study/studyCourseSelect";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;

        public StudyCourseSelectController(IAuthManager authManager,
            IStudyManager studyManager,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseWareRepository studyCourseWareRepository)
        {
            _authManager = authManager;
            _studyManager = studyManager;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseWareRepository = studyCourseWareRepository;
        }
        public class GetRequest
        {
            public string Keyword { get; set; }
            public string Type { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<StudyCourse> List { get; set; }
            public int Total { get; set; }

        }
    }
}
