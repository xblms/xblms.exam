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
    public partial class ExamTmCorrectionController : ControllerBase
    {
        private const string Route = "exam/correction";
        private const string RouteAudit = "exam/correction/audit";
        private readonly IAuthManager _authManager;
        private readonly IExamTmCorrectionRepository _examTmCorrectionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdministratorRepository _administratorRepository;

        public ExamTmCorrectionController(IAuthManager authManager, IExamTmCorrectionRepository examTmCorrectionRepository, IUserRepository userRepository, IAdministratorRepository administratorRepository)
        {
            _authManager = authManager;
            _examTmCorrectionRepository = examTmCorrectionRepository;
            _userRepository = userRepository;
            _administratorRepository = administratorRepository;
        }
        public class GetAuditResult
        {
            public List<Select<string>> StatusList { get; set; }
            public GetAuditItemResult Item { get; set; }
        }
        public class GetAuditItemResult
        {
            public int Id { get; set; }
            public string AuditResaon { get; set; }
            public ExamTmCorrectionAuditType AuditStatus { get; set; }
        }
        public class GetRequest
        {
            public string KeyWords { get; set; }
            public string Status { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetResult
        {
            public List<Select<string>> StatusList { get; set; }
            public IEnumerable<ExamTmCorrection> List { get; set; }
            public int Total { get; set; }
        }

        public class GetSetRequest
        {
            public int Id { get; set; }
            public bool Value { get; set; }
        }
    }
}
