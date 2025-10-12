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
    public partial class ExamTxController : ControllerBase
    {
        private const string Route = "exam/examTx";
        private const string RouteDelete = "exam/examTx/del";

        private readonly IAuthManager _authManager;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamTmRepository _examTmRepository;

        public ExamTxController(IAuthManager authManager, IExamTxRepository examTxRepository, IExamTmRepository examTmRepository)
        {
            _authManager = authManager;
            _examTxRepository = examTxRepository;
            _examTmRepository = examTmRepository;
        }
        public class GetRequest
        {
            public string Name { get; set; }
        }
        public class GetResult
        {
            public bool Operate { get; set; }
            public List<ExamTx> Items { get; set; }
        }
    }
}
