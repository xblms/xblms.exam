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
    public partial class ExamTxEditController : ControllerBase
    {
        private const string Route = "exam/examTxEdit";
        private const string RouteAdd = Route + "/add";
        private const string RouteUpdate = Route + "/update";

        private readonly IAuthManager _authManager;
        private readonly IExamTxRepository _examTxRepository;

        public ExamTxEditController(IAuthManager authManager, IExamTxRepository examTxRepository)
        {
            _authManager = authManager;
            _examTxRepository = examTxRepository;
        }
        public class GetResult
        {
            public ExamTx Item { get; set; }
            public IEnumerable<Select<string>> TypeList { get; set; }
        }
    }
}
