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
    public partial class ExamPaperEditController : ControllerBase
    {
        private const string Route = "exam/examPaperEdit";
        private const string RouteGetConfig = Route + "/getConfig";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperTreeRepository _examPaperTreeRepository;
        private readonly IExamTmGroupRepository _examTmGroupRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamManager _examManager;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;

        public ExamPaperEditController(IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperTreeRepository examPaperTreeRepository,
            IExamTmGroupRepository examTmGroupRepository,
            IExamTxRepository examTxRepository,
            IExamCerRepository examCerRepository,
            IExamManager examManager,
            IUserGroupRepository userGroupRepository,
            IExamTmRepository examTmRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperStartRepository examPaperStartRepository)
        {
            _authManager = authManager;
            _examPaperRepository = examPaperRepository;
            _examPaperTreeRepository = examPaperTreeRepository;
            _examTmGroupRepository = examTmGroupRepository;
            _examTxRepository = examTxRepository;
            _examCerRepository = examCerRepository;
            _examManager = examManager;
            _userGroupRepository = userGroupRepository;
            _examTmRepository = examTmRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperStartRepository = examPaperStartRepository;
        }
        public class GetConfigRequest
        {
            public List<int> TxIds { get; set; }
            public List<int> TmGroupIds { get; set; }
        }
        public class GetConfigResult
        {
            public List<ExamPaperRandomConfig> Items { get; set; }
        }
        public class GetResult
        {
            public ExamPaper Item { get; set; }
            public List<Cascade<int>> PaperTree { get; set; }
            public List<ExamTx> TxList { get; set; }
            public List<ExamTmGroup> TmGroupList { get; set; }
            public List<ExamTmGroup> TmFixedGroupList { get; set; }
            public List<UserGroup> UserGroupList { get; set; }
            public List<ExamCer> CerList { get; set; }
            public List<ExamPaperRandomConfig> ConfigList { get; set; }
        }
        public class GetSubmitRequest
        {
            public SubmitType SubmitType { get; set; }
            public ExamPaper Item { get; set; }
            public List<ExamPaperRandomConfig> ConfigList { get; set; }
            public bool IsClear { get; set; }
            public bool IsUpdateDateTime { get; set; }
            public bool IsUpdateExamTimes { get; set; }
        }

    }
}

