using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class ExamTmCorrectionController : ControllerBase
    {
        private const string Route = "exam/ExamTmCorrection";
        private const string RouteSubmit = Route + "/submit";
        private const string RouteLog = Route + "/log";
        private const string RouteView = Route + "/layerView";

        private readonly IAuthManager _authManager;
        private readonly IExamTmCorrectionRepository _examTmCorrectionRepository;
        private readonly IExamTmRepository _examTmRepository;
        private readonly IExamManager _examManager;
        private readonly IOrganManager _organManager;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IUserRepository _userRepository;

        public ExamTmCorrectionController(IAuthManager authManager, IExamTmCorrectionRepository examTmCorrectionRepository, IExamTmRepository examTmRepository, IExamManager examManager, IOrganManager organManager, IExamTxRepository examTxRepository, IUserRepository userRepository)
        {
            _authManager = authManager;
            _examTmCorrectionRepository = examTmCorrectionRepository;
            _examTmRepository = examTmRepository;
            _examManager = examManager;
            _organManager = organManager;
            _examTxRepository = examTxRepository;
            _userRepository = userRepository;
        }

        public class GetRequest
        {
            public string Reason { get; set; }
            public int TmId { get; set; }
            public int ExamPaperId { get; set; }
        }
        public class GetResult
        {
            public int Total { get; set; }
            public List<ExamTmCorrection> List { get; set; }
            public string Title { get; set; }
        }


        public class GetLogRequest
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string Keywords { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
        }
        public class GetLogResult
        {
            public List<ExamTmCorrection> List { get; set; }
            public int Total { get; set; }
        }

        public class GetViewResult
        {
            public ExamTm ItemNew { get; set; }
            public ExamTm Item { get; set; }
            public ExamTmCorrection Info { get; set; }
        }
    }
}
