using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
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
    public partial class ExamTmGroupController : ControllerBase
    {
        private const string Route = "exam/examTmGroup";
        private const string RouteDelete = Route + "/delete";

        private const string RouteEditGet = Route + "/editGet";
        private const string RouteEditPost = Route + "/editPost";
        private const string RouteTmTotal = Route + "/tmTotal";

        private readonly IAuthManager _authManager;
        private readonly IExamTmGroupRepository _examTmGroupRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamManager _examManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public ExamTmGroupController(IAuthManager authManager, IExamTmGroupRepository examTmGroupRepository, IExamManager examManager, IExamTxRepository examTxRepository, IAdministratorRepository administratorRepository, IExamTmRepository examTmRepository, IExamPaperRepository examPaperRepository, IUserGroupRepository userGroupRepository)
        {
            _authManager = authManager;
            _examTmGroupRepository = examTmGroupRepository;
            _examManager = examManager;
            _examTxRepository = examTxRepository;
            _administratorRepository = administratorRepository;
            _examTmRepository = examTmRepository;
            _examPaperRepository = examPaperRepository;
            _userGroupRepository = userGroupRepository;
        }
        public class GetRequest
        {
            public string Search { get; set; }
        }
        public class GetResult
        {
            public IEnumerable<ExamTmGroup> Groups { get; set; }
        }

        public class GetEditResult
        {
            public ExamTmGroup Group { get; set; }
            public List<Cascade<int>> TmTree { get; set; }
            public List<ExamTx> TxList { get; set; }
            public List<Select<string>> GroupTypeSelects { get; set; }
            public List<UserGroup> UserGroups { get; set; }
            public bool WithTree { get; set; }
        }

        public class GetEditRequest
        {
            public ExamTmGroup Group { get; set; }
        }

        public class GetTmTotalResult
        {
            public int TmTotal { get; set; }
            public int UseTotal { get; set; }
        }
    }
}
