using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;
namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamTmSelectController : ControllerBase
    {
        private const string Route = "exam/examTmSelect";
        private const string RouteGetIn = Route + "/getIn";
        private const string RouteSelect = Route + "/setGroupTm";
        private const string RouteRemove = Route + "/delGroupTm";

        private readonly IAuthManager _authManager;
        private readonly IExamTmTreeRepository _examTmTreeRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamManager _examManager;
        private readonly IExamTmGroupRepository _examTmGroupRepository;

        public ExamTmSelectController(IAuthManager authManager,
            IExamManager examManager,
            IExamTmTreeRepository examTmTreeRepository,
            IExamTmRepository examTmRepository,
            IExamTmGroupRepository examTmGroupRepository)
        {
            _examManager = examManager;
            _authManager = authManager;
            _examTmTreeRepository = examTmTreeRepository;
            _examTmRepository = examTmRepository;
            _examTmGroupRepository = examTmGroupRepository;
        }
        public class GetSeletRemoveRequest
        {
            public List<int> Ids { get; set; }
            public int Id { get; set; }
        }
        public class GetSelectResult
        {
            public List<ExamTm> Items { get; set; }
        }
        public class GetSearchResults
        {
            public List<ExamTm> Items { get; set; }
            public int Total { get; set; }

        }
        public class GetSearchRequest
        {
            public int Id { get; set; }
            public bool TreeIsChildren { get; set; }
            public int TreeId { get; set; }
            public int TxId { get; set; }
            public int Nandu { get; set; }
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public string Order { get; set; }
            public string OrderType { get; set; }
            public bool? IsStop { get; set; }
        }
    }
}
