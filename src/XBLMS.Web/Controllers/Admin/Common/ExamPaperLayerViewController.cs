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
    public partial class ExamPaperLayerViewController : ControllerBase
    {
        private const string Route = "common/examPaperLayerView";

        private readonly IExamTmRepository _examTmRepository;
        private readonly IAuthManager _authManager;
        private readonly IExamManager _examManager;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperRandomRepository _examPaperRandomRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;

        public ExamPaperLayerViewController(IAuthManager authManager,
            IExamTmRepository examTmRepository, IExamManager examManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperRandomRepository examPaperRandomRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository, IExamPaperRandomTmRepository examPaperRandomTmRepository)
        {
            _authManager = authManager;
            _examTmRepository = examTmRepository;
            _examManager = examManager;
            _examPaperRepository = examPaperRepository;
            _examPaperRandomRepository = examPaperRandomRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
        }
        public class GetRequest
        {
            public int Id { get;set; }
            public int RandomId { get; set; }
        }
        public class GetResult
        {
            public ExamPaper Item { get; set; }
            public List<ExamPaperRandomConfig> TxList { get; set; }
            public List<int> RandomIds { get; set; }
            public int RandomId { get; set; }
        }
    }
}
