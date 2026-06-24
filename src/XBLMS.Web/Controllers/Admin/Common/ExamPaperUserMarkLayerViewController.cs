using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Common
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamPaperUserMarkLayerViewController : ControllerBase
    {
        private const string Route = "common/examPaperUserMarkLayerView";

        private readonly IAdministratorRepository _administratorRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamManager _examManager;
        private readonly IExamTxRepository _examTxRepository;

        public ExamPaperUserMarkLayerViewController(IExamPaperRepository examPaperRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository,
            IExamManager examManager,
            IExamPaperStartRepository examPaperStartRepository,
            IExamTxRepository examTxRepository,
            IAdministratorRepository administratorRepository)
        {
            _examPaperRepository = examPaperRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _examManager = examManager;
            _examPaperStartRepository = examPaperStartRepository;
            _examTxRepository = examTxRepository;
            _administratorRepository = administratorRepository;
        }
        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamPaper Item { get; set; }
            public List<ExamPaperRandomConfig> TxList { get; set; }
        }
    }
}
