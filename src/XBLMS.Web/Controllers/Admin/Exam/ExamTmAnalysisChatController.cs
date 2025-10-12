using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamTmAnalysisChatController : ControllerBase
    {
        private const string Route = "exam/examTmAnalysisChat";

        private readonly IAuthManager _authManager;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamTmAnalysisTmRepository _examTmAnalysisTmRepository;

        public ExamTmAnalysisChatController(IAuthManager authManager,IExamTmAnalysisTmRepository examTmAnalysisTmRepository,IExamTxRepository examTxRepository)
        {
            _authManager = authManager;
            _examTmAnalysisTmRepository = examTmAnalysisTmRepository;
            _examTxRepository = examTxRepository;
        }

        public class GetResult
        {
            public List<string> TxLabels { get; set; }
            public List<int> TxValues { get; set; }
            public List<string> NdLabels { get; set; }
            public List<int> NdValues { get; set; }
            public List<string> ZsdLabels { get; set; }
            public List<int> ZsdValues { get; set; }
        }

    }
}
