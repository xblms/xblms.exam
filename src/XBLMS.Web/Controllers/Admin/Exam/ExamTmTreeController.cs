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
    public partial class ExamTmTreeController : ControllerBase
    {
        private const string Route = "exam/examTmTree";
        private const string RouteDelete = Route + "/del";
        private const string RouteAdd = Route + "/add";
        private const string RouteUpdate = Route + "/update";

        private readonly IAuthManager _authManager;
        private readonly IExamTmTreeRepository _examTmTreeRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamManager _examManager;
        private readonly IExamTmGroupRepository _examTmGroupRepository;

        public ExamTmTreeController(IAuthManager authManager, IExamTmTreeRepository examTmTreeRepository, IExamTxRepository examTxRepository, IExamManager examManager, IExamTmGroupRepository examTmGroupRepository)
        {
            _authManager = authManager;
            _examTmTreeRepository = examTmTreeRepository;
            _examTxRepository = examTxRepository;
            _examManager = examManager;
            _examTmGroupRepository = examTmGroupRepository;
        }
        public class GetResult
        {
            public List<ExamTx> TxList { get; set; }
            public List<Select<string>> OrderTypeList { get; set; }
            public List<Cascade<int>> Items { get; set; }
            public List<ExamTmGroup> TmGroups { get; set; }
        }
        public class GetTreeNamesRequest
        {
            public string Names { get; set; }
            public int ParentId { get; set; }
        }
    }
}
