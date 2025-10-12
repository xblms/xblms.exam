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
    public partial class StudyCourseFilesGroupEditController : ControllerBase
    {
        private const string Route = "study/studyCourseFilesGroupEdit";
        private const string RouteAdd = Route + "/add";
        private const string RouteUpdate = Route + "/update";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IPathManager _pathManager;
        private readonly IStudyCourseFilesGroupRepository _studyCourseFilesGroupRepository;

        public StudyCourseFilesGroupEditController(ISettingsManager settingsManager, IAuthManager authManager, IPathManager pathManager, IStudyCourseFilesGroupRepository studyCourseFilesGroupRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _pathManager = pathManager;
            _studyCourseFilesGroupRepository = studyCourseFilesGroupRepository;
        }

        public class GetRequest
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
        }

        public class CreateRequest
        {
            public string GroupName { get; set; }
            public int ParentId { get; set; }
        }

        public class CreateResult
        {
            public List<StudyCourseFilesGroup> Groups { get; set; }
        }

        public class UpdateRequest
        {
            public int Id { get; set; }
            public string GroupName { get; set; }
            public int ParentId { get; set; }
        }

        public class UpdateResult
        {
            public List<StudyCourseFilesGroup> Groups { get; set; }
        }
    }
}
