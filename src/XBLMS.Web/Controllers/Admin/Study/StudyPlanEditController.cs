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
    public partial class StudyPlanEditController : ControllerBase
    {
        private const string Route = "study/studyPlanEdit";
        private const string RouteUpload = Route + "/upload";

        private readonly IAuthManager _authManager;
        private readonly IStudyManager _studyManager;
        private readonly IPathManager _pathManager;
        private readonly IUploadManager _uploadManager;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IStudyPlanRepository _studyPlanRepository;
        private readonly IStudyPlanCourseRepository _studyPlanCourseRepository;
        private readonly IStudyCourseRepository _studyCourseRepository;
        private readonly IStudyCourseTreeRepository _studyCourseTreeRepository;
        private readonly IStudyCourseFilesRepository _studyCourseFilesRepository;
        private readonly IStudyCourseWareRepository _studyCourseWareRepository;
        private readonly IStudyPlanUserRepository _studyPlanUserRepository;

        public StudyPlanEditController(IAuthManager authManager,
            IPathManager pathManager,
            IUploadManager uploadManager,
            IStudyManager studyManager,
            IUserGroupRepository userGroupRepository,
            IStudyPlanRepository studyPlanRepository,
            IStudyPlanCourseRepository studyPlanCourseRepository,
            IStudyCourseRepository studyCourseRepository,
            IStudyCourseFilesRepository studyCourseFilesRepository,
            IStudyCourseTreeRepository studyCourseTreeRepository,
            IStudyCourseWareRepository studyCourseWareRepository,
            IStudyPlanUserRepository studyPlanUserRepository)
        {
            _authManager = authManager;
            _pathManager = pathManager;
            _uploadManager = uploadManager;
            _studyManager = studyManager;
            _userGroupRepository = userGroupRepository;
            _studyPlanRepository = studyPlanRepository;
            _studyPlanCourseRepository = studyPlanCourseRepository;
            _studyCourseRepository = studyCourseRepository;
            _studyCourseTreeRepository = studyCourseTreeRepository;
            _studyCourseFilesRepository = studyCourseFilesRepository;
            _studyCourseWareRepository = studyCourseWareRepository;
            _studyPlanUserRepository = studyPlanUserRepository;
        }
        public class GetResult
        {
            public StudyPlan Item { get; set; }
            public List<UserGroup> UserGroupList { get; set; }
            public List<StudyPlanCourse> CourseList { get; set; }
            public List<StudyPlanCourse> CourseSelectList { get; set; }
        }
        public class GetSubmitRequest
        {
            public StudyPlan Item { get; set; }
            public List<StudyPlanCourse> CourseList { get; set; }
            public List<StudyPlanCourse> CourseSelectList { get; set; }
        }

    }
}

