using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Study
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class StudyCourseEditController : ControllerBase
    {
        private const string Route = "study/studyCourseEdit";
        private const string RouteUpload = Route + "/upload";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IPathManager _pathManager;
        private readonly IUploadManager _uploadManager;
        private readonly IConfigRepository _configRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseTreeRepository _studyCourseTreeRepository;
        private readonly IStudyCourseFilesRepository _studyCourseFilesRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;
        private readonly IStudyCourseUserRepository _studyCourseUserRepository;

        public StudyCourseEditController(IAuthManager authManager,
            IPathManager pathManager,
            IUploadManager uploadManager,
            IStudyManager studyManager,
            IConfigRepository configRepository,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseFilesRepository studyCourseFilesRepository,
            IStudyCourseTreeRepository studyCourseTreeRepository,
            IStudyCourseWareRepository studyCourseWareRepository,
            IStudyCourseUserRepository studyCourseUserRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _uploadManager = uploadManager;
            _studyManager = studyManager;
            _configRepository = configRepository;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseTreeRepository = studyCourseTreeRepository;
            _studyCourseFilesRepository = studyCourseFilesRepository;
            _studyCourseWareRepository = studyCourseWareRepository;
            _studyCourseUserRepository = studyCourseUserRepository;
        }
        public class GetRequest
        {
            public int Id { get; set; }
            public bool Face { get; set; }
        }
        public class GetResult
        {
            public StudyCourse Item { get; set; }
            public List<Cascade<int>> Tree { get; set; }
            public List<StudyCourseWare> WareList { get; set; }
            public List<string> MarkList { get; set; }
        }
        public class GetSubmitRequest
        {
            public StudyCourse Item { get; set; }
            public List<StudyCourseWare> WareList { get; set; }
        }

    }
}

