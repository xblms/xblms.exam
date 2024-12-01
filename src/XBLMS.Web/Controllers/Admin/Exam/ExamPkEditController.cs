using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamPkEditController : ControllerBase
    {
        private const string Route = "exam/examPkEdit";

        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IOrganManager _organManager;

        private readonly IExamPkRepository  _examPkRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IExamTmGroupRepository _tmGroupRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamPkUserRepository _examPkUserRepository;
        private readonly IExamPkRoomRepository _examPkRoomRepository;

        private readonly IUserRepository _userRepository;

        public ExamPkEditController(IAuthManager authManager,
            IOrganManager organManager,
            IExamManager examManager,
            IExamPkRepository examPkRepository,
            IExamTmGroupRepository examTmGroupRepository,
            IUserGroupRepository userGroupRepository,
            IUserRepository userRepository,
            IExamTmRepository examTmRepository,
            IExamTxRepository examTxRepository,
            IExamPkUserRepository examPkUserRepository,
            IExamPkRoomRepository examPkRoomRepository)
        {
            _authManager = authManager;
            _organManager = organManager;
            _examManager = examManager;
            _examPkRepository = examPkRepository;
            _userGroupRepository = userGroupRepository;
            _tmGroupRepository = examTmGroupRepository;
            _userRepository = userRepository;
            _examTmRepository = examTmRepository;
            _examTxRepository = examTxRepository;
            _examPkUserRepository = examPkUserRepository;
            _examPkRoomRepository = examPkRoomRepository;
        }
        public class GetResult
        {
            public ExamPk Item { get; set; }
            public List<UserGroup> UserGroupList { get; set; }
            public List<ExamTmGroup> TmGroupList { get; set; }
            public List<Select<string>> RuleList { get; set; }
        }
        public class GetSubmitRequest
        {
            public ExamPk Item { get; set; }
        }

    }
}

