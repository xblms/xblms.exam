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
    public partial class ExamPaperUserMarkController : ControllerBase
    {
        private const string Route = "common/examPaperUserMark";

        private const string RouteSave = Route + "/save";
        private const string RouteSubmit = Route + "/submit";

        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly ICreateManager _createManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperRandomConfigRepository _examPaperRandomConfigRepository;
        private readonly IExamPaperRandomRepository _examPaperRandomRepository;
        private readonly IExamPaperRandomTmRepository _examPaperRandomTmRepository;
        private readonly IExamPaperAnswerRepository _examPaperAnswerRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamManager _examManager;
        private readonly IUserRepository _userRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamPaperAnswerSmallRepository _examPaperAnswerSmallRepository;

        public ExamPaperUserMarkController(IConfigRepository configRepository,
            ICreateManager createManager,
            IAuthManager authManager,
            IExamPaperRepository examPaperRepository,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperRandomConfigRepository examPaperRandomConfigRepository,
            IExamPaperRandomRepository examPaperRandomRepository,
            IExamPaperRandomTmRepository examPaperRandomTmRepository,
            IExamManager examManager,
            IExamPaperAnswerRepository examPaperAnswerRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IUserRepository userRepository,
            IExamTxRepository examTxRepository,
            IExamPaperAnswerSmallRepository examPaperAnswerSmallRepository)
        {
            _configRepository = configRepository;
            _authManager = authManager;
            _createManager = createManager;
            _examPaperRepository = examPaperRepository;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperRandomConfigRepository = examPaperRandomConfigRepository;
            _examPaperRandomRepository = examPaperRandomRepository;
            _examPaperRandomTmRepository = examPaperRandomTmRepository;
            _examManager = examManager;
            _examPaperAnswerRepository = examPaperAnswerRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _userRepository = userRepository;
            _examTxRepository = examTxRepository;
            _examPaperAnswerSmallRepository = examPaperAnswerSmallRepository;
        }
        public class GetResult
        {
            public string Watermark { get; set; }
            public ExamPaper Item { get; set; }
            public List<ExamPaperRandomConfig> TxList { get; set; }
        }

        public class GetMarkRequest
        {
            public int StartId { get; set; }
            public List<ExamPaperAnswer> List { get; set; }
            public List<ExamPaperAnswerSmall> SmallList { get; set; }
        }
    }
}
