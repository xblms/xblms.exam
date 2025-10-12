using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamPaperTreeController : ControllerBase
    {
        private const string Route = "exam/examPaperTree";
        private const string RouteDelete = Route + "/del";
        private const string RouteAdd = Route + "/add";
        private const string RouteUpdate = Route + "/update";

        private readonly IAuthManager _authManager;
        private readonly IExamPaperTreeRepository _examPaperTreeRepository;
        private readonly IExamManager _examManager;

        public ExamPaperTreeController(IAuthManager authManager, IExamPaperTreeRepository examPaperTreeRepository, IExamManager examManager)
        {
            _authManager = authManager;
            _examPaperTreeRepository = examPaperTreeRepository;
            _examManager = examManager;
        }
        public class GetResult
        {
            public List<Cascade<int>> Items { get; set; }
        }
        public class GetTreeNamesRequest
        {
            public string Names { get; set; }
            public int ParentId { get; set; }
        }
    }
}
